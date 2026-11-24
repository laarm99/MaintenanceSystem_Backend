using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class ProceduresImporter : EntityImporter
    {
        public override ImportType Type => ImportType.Procedures;

        public override int Order => 5;

        public override IReadOnlyList<ImportType> DependsOn => [ImportType.EquipmentTypes];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("CODE", true, "Código único del procedimiento (máx. 30).", "PROC-SRV-01"),
            ImportColumn.Text("NAME", true, "Nombre del procedimiento (máx. 150).", "Mantenimiento preventivo de servidor"),
            ImportColumn.Text("DESCRIPTION", false, "Descripción (máx. 1000)."),
            ImportColumn.Text("EQUIPMENT_TYPE_CODE", false, "Código del tipo de equipo al que aplica (debe existir).", "SRV"),
            ImportColumn.Integer("ESTIMATED_DURATION_MINUTES", false, "Duración estimada en minutos. Vacío = 0.", "60"),
            ImportColumn.Boolean("IS_ACTIVE", "Activo. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var types = await context.Db.EquipmentTypes.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var existing = await context.Db.Procedures.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var code = row.Code("CODE", 30, true);
                var name = row.Text("NAME", 150, true);
                var description = row.Text("DESCRIPTION", 1000);
                var typeCode = row.Code("EQUIPMENT_TYPE_CODE", 20);
                var duration = row.Int("ESTIMATED_DURATION_MINUTES", false) ?? 0;
                var isActive = row.Bool("IS_ACTIVE", true);

                EquipmentType? type = null;
                if (typeCode is not null && !types.TryGetValue(typeCode, out type))
                    row.AddError($"EQUIPMENT_TYPE_CODE: el tipo de equipo '{typeCode}' no existe.");

                row.Key = code ?? string.Empty;
                if (code is not null) row.EnsureUnique(seen, code);
                if (row.HasErrors) continue;

                var isNew = !existing.TryGetValue(code!, out var procedure);
                procedure ??= new Procedure { Code = code! };
                procedure.Name = name!;
                procedure.Description = description;
                procedure.EquipmentTypeId = type?.Id;
                procedure.EstimatedDurationMinutes = duration;
                procedure.IsActive = isActive;

                if (isNew) existing[code!] = procedure;
                Track(context, row, procedure, isNew);
            }
        }
    }
}