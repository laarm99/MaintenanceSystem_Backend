using ClosedXML.Excel;
using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Application.Modules.Imports.DTOs;
using MaintenanceSystem.Common.Results;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal sealed class ImportService : IImportService
    {
        public const int MaxRows = 5000;

        private static readonly XLColor RequiredHeaderColor = XLColor.FromHtml("#FB8421");
        private static readonly XLColor OptionalHeaderColor = XLColor.FromHtml("#162852");

        private readonly ProjectDbContext _db;
        private readonly IReadOnlyDictionary<ImportType, IEntityImporter> _importers;

        public ImportService(ProjectDbContext db, IEnumerable<IEntityImporter> importers)
        {
            _db = db;
            _importers = importers.ToDictionary(x => x.Type);
        }

        public IReadOnlyList<ImportDefinitionDTO> GetDefinitions() =>
            _importers.Values
                .OrderBy(x => x.Order)
                .Select(x => new ImportDefinitionDTO
                {
                    Type = x.Type,
                    Order = x.Order,
                    DependsOn = x.DependsOn,
                    Columns = x.Columns.Select(c => c.ToDTO()).ToList()
                })
                .ToList();

        public ServiceResult<byte[]> BuildTemplate(ImportType type)
        {
            if (!_importers.TryGetValue(type, out var importer))
                return ServiceResult<byte[]>.NotFound($"No existe una carga para '{type}'.");

            using var workbook = new XLWorkbook();
            var data = workbook.AddWorksheet(ExcelImportReader.DataSheetName);
            var lastDataRow = MaxRows + 1;

            for (var i = 0; i < importer.Columns.Count; i++)
            {
                var column = importer.Columns[i];
                var columnNumber = i + 1;
                var header = data.Cell(1, columnNumber);

                header.Value = column.Name;
                header.Style.Font.Bold = true;
                header.Style.Font.FontColor = XLColor.White;
                header.Style.Fill.BackgroundColor = column.Required ? RequiredHeaderColor : OptionalHeaderColor;
                header.CreateComment().AddText(column.Description);

                var body = data.Range(2, columnNumber, lastDataRow, columnNumber);
                if (column.Kind == ImportColumnKind.Date)
                    body.Style.NumberFormat.Format = "yyyy-mm-dd";
                else if (column.Kind == ImportColumnKind.Text)
                    body.Style.NumberFormat.Format = "@";

                if (column.AllowedValues is { Count: > 0 })
                    body.CreateDataValidation().List($"\"{string.Join(",", column.AllowedValues)}\"", true);

                data.Column(columnNumber).Width = Math.Max(14, column.Name.Length + 4);
            }

            data.SheetView.FreezeRows(1);

            BuildInstructionsSheet(workbook.AddWorksheet("Instrucciones"), importer);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return ServiceResult<byte[]>.Success(stream.ToArray());
        }

        public async Task<ServiceResult<ImportResultDTO>> ImportAsync(
            ImportType type,
            Stream file,
            string fileName,
            bool commit,
            string userName,
            CancellationToken cancellationToken)
        {
            if (!_importers.TryGetValue(type, out var importer))
                return ServiceResult<ImportResultDTO>.NotFound($"No existe una carga para '{type}'.");

            var (rows, fileErrors) = ExcelImportReader.Read(file, importer.Columns);

            if (fileErrors.Count > 0)
                return ServiceResult<ImportResultDTO>.BadRequest("El archivo no tiene el formato esperado.", fileErrors);

            if (rows.Count == 0)
                return ServiceResult<ImportResultDTO>.BadRequest("El archivo no contiene registros.");

            if (rows.Count > MaxRows)
                return ServiceResult<ImportResultDTO>.BadRequest($"El archivo excede el máximo de {MaxRows} registros.");

            try
            {
                await importer.ProcessAsync(rows, new ImportContext(_db, userName), cancellationToken);

                var result = new ImportResultDTO
                {
                    Type = type,
                    FileName = fileName,
                    TotalRows = rows.Count,
                    Rows = rows.Select(x => x.ToDTO()).ToList()
                };
                result.CreatedCount = result.Rows.Count(x => x.Action == ImportRowAction.Create);
                result.UpdatedCount = result.Rows.Count(x => x.Action == ImportRowAction.Update);
                result.UnchangedCount = result.Rows.Count(x => x.Action == ImportRowAction.Unchanged);
                result.ErrorCount = result.Rows.Count(x => x.Action == ImportRowAction.Error);

                // All-or-nothing: nothing is saved while any row has errors.
                if (commit && result.ErrorCount == 0)
                {
                    await _db.SaveChangesAsync(cancellationToken);
                    result.Committed = true;
                }

                return ServiceResult<ImportResultDTO>.Success(result);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<ImportResultDTO>.Conflict(
                    $"No se pudo guardar la carga: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                _db.ChangeTracker.Clear();
            }
        }

        private static void BuildInstructionsSheet(IXLWorksheet sheet, IEntityImporter importer)
        {
            var row = 1;
            sheet.Cell(row, 1).Value = $"Carga: {importer.Type}";
            sheet.Cell(row, 1).Style.Font.Bold = true;
            sheet.Cell(row, 1).Style.Font.FontSize = 14;

            row += 2;
            string[] notes =
            [
                $"Capture los registros en la hoja '{ExcelImportReader.DataSheetName}' a partir de la fila 2 (máximo {MaxRows}).",
                "Encabezados naranjas = obligatorios; azules = opcionales. No cambie los nombres de los encabezados.",
                "Si el registro ya existe se actualiza; si no, se crea. Las celdas opcionales vacías limpian el valor existente.",
                "La carga es todo o nada: si una fila tiene errores no se guarda ninguna.",
                importer.DependsOn.Count > 0
                    ? $"Antes debe cargar: {string.Join(", ", importer.DependsOn)}."
                    : "Esta carga no depende de otras."
            ];
            foreach (var note in notes)
                sheet.Cell(row++, 1).Value = $"• {note}";

            row++;
            string[] headers = ["Columna", "Obligatoria", "Tipo", "Descripción", "Valores permitidos", "Ejemplo"];
            for (var i = 0; i < headers.Length; i++)
            {
                var cell = sheet.Cell(row, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = OptionalHeaderColor;
            }

            foreach (var column in importer.Columns)
            {
                row++;
                sheet.Cell(row, 1).Value = column.Name;
                sheet.Cell(row, 2).Value = column.Required ? "SI" : "NO";
                sheet.Cell(row, 3).Value = column.Kind.ToString();
                sheet.Cell(row, 4).Value = column.Description;
                sheet.Cell(row, 5).Value = column.AllowedValues is null ? string.Empty : string.Join(", ", column.AllowedValues);
                sheet.Cell(row, 6).Value = column.Example ?? string.Empty;
            }

            sheet.Column(1).Width = 30;
            sheet.Column(2).Width = 12;
            sheet.Column(3).Width = 10;
            sheet.Column(4).Width = 70;
            sheet.Column(5).Width = 50;
            sheet.Column(6).Width = 30;
        }
    }
}