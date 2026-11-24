using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class ProcedureChecklistItemsImporter : EntityImporter
    {
        public override ImportType Type => ImportType.ProcedureChecklistItems;

        public override int Order => 6;

        public override IReadOnlyList<ImportType> DependsOn => [ImportType.Procedures];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("PROCEDURE_CODE", true, "Código del procedimiento (debe existir).", "PROC-SRV-01"),
            ImportColumn.Integer("SORT_ORDER", true, "Número de paso, único por procedimiento (1, 2, 3...).", "1"),
            ImportColumn.Text("DESCRIPTION", true, "Actividad a revisar (máx. 300).", "Limpieza física de ventiladores"),
            ImportColumn.Boolean("IS_REQUIRED", "Obligatorio para cerrar el mantenimiento. Vacío = SI.", "SI"),
            ImportColumn.Boolean("IS_ACTIVE", "Activo. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var procedures = await context.Db.Procedures.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var existing = (await context.Db.ProcedureChecklistItems.ToListAsync(cancellationToken))
                .ToDictionary(x => $"{x.ProcedureId}|{x.SortOrder}");
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var procedureCode = row.Code("PROCEDURE_CODE", 30, true);
                var sortOrder = row.Int("SORT_ORDER", true, 1);
                var description = row.Text("DESCRIPTION", 300, true);
                var isRequired = row.Bool("IS_REQUIRED", true);
                var isActive = row.Bool("IS_ACTIVE", true);

                Procedure? procedure = null;
                if (procedureCode is not null && !procedures.TryGetValue(procedureCode, out procedure))
                    row.AddError($"PROCEDURE_CODE: el procedimiento '{procedureCode}' no existe.");

                row.Key = $"{procedureCode}/{sortOrder}";
                if (procedureCode is not null && sortOrder is not null) row.EnsureUnique(seen, row.Key);
                if (row.HasErrors) continue;

                var key = $"{procedure!.Id}|{sortOrder}";
                var isNew = !existing.TryGetValue(key, out var item);
                item ??= new ProcedureChecklistItem { ProcedureId = procedure.Id, SortOrder = sortOrder!.Value };
                item.Description = description!;
                item.IsRequired = isRequired;
                item.IsActive = isActive;

                if (isNew) existing[key] = item;
                Track(context, row, item, isNew);
            }
        }
    }
}