using ClosedXML.Excel;
using System.Globalization;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal static class ExcelImportReader
    {
        public const string DataSheetName = "Datos";

        public static (List<ImportRow> Rows, List<string> Errors) Read(Stream stream, IReadOnlyList<ImportColumn> columns)
        {
            var rows = new List<ImportRow>();
            var errors = new List<string>();

            XLWorkbook workbook;
            try
            {
                workbook = new XLWorkbook(stream);
            }
            catch (Exception)
            {
                errors.Add("El archivo no es un Excel (.xlsx) válido.");
                return (rows, errors);
            }

            using (workbook)
            {
                var sheet = workbook.Worksheets.FirstOrDefault(x => x.Name.Equals(DataSheetName, StringComparison.OrdinalIgnoreCase))
                    ?? workbook.Worksheets.First();

                var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (var cell in sheet.Row(1).CellsUsed())
                {
                    var header = cell.GetString().Trim().TrimEnd('*').Trim();
                    if (header.Length > 0) headerMap.TryAdd(header, cell.Address.ColumnNumber);
                }

                foreach (var column in columns.Where(x => x.Required && !headerMap.ContainsKey(x.Name)))
                    errors.Add($"Falta la columna obligatoria '{column.Name}'.");

                if (errors.Count > 0) return (rows, errors);

                var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;
                for (var rowNumber = 2; rowNumber <= lastRow; rowNumber++)
                {
                    var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                    foreach (var column in columns)
                    {
                        if (headerMap.TryGetValue(column.Name, out var columnNumber))
                            values[column.Name] = ReadCell(sheet.Cell(rowNumber, columnNumber));
                    }

                    if (values.Values.All(x => x is null)) continue;

                    rows.Add(new ImportRow(rowNumber, values));
                }
            }

            return (rows, errors);
        }

        private static string? ReadCell(IXLCell cell)
        {
            if (cell.IsEmpty()) return null;

            var value = cell.Value;

            if (value.IsDateTime) return value.GetDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (value.IsNumber) return value.GetNumber().ToString(CultureInfo.InvariantCulture);
            if (value.IsBoolean) return value.GetBoolean() ? "SI" : "NO";
            if (value.IsError) return "#ERROR";

            var text = cell.GetString().Trim();
            return text.Length == 0 ? null : text;
        }
    }
}