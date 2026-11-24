using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class EquipmentTypesImporter : EntityImporter
    {
        public override ImportType Type => ImportType.EquipmentTypes;

        public override int Order => 4;

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("CODE", true, "Código único del tipo de equipo (máx. 20).", "SRV"),
            ImportColumn.Text("NAME", true, "Nombre del tipo de equipo (máx. 100).", "Servidor"),
            ImportColumn.Enum<MaintenanceFrequency>("DEFAULT_FREQUENCY", false, "Frecuencia sugerida para equipos de este tipo.", nameof(MaintenanceFrequency.Quarterly)),
            ImportColumn.Boolean("IS_ACTIVE", "Activo. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var existing = await context.Db.EquipmentTypes.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var code = row.Code("CODE", 20, true);
                var name = row.Text("NAME", 100, true);
                var frequency = row.Enum<MaintenanceFrequency>("DEFAULT_FREQUENCY", false);
                var isActive = row.Bool("IS_ACTIVE", true);

                row.Key = code ?? string.Empty;
                if (code is not null) row.EnsureUnique(seen, code);
                if (row.HasErrors) continue;

                var isNew = !existing.TryGetValue(code!, out var type);
                type ??= new EquipmentType { Code = code! };
                type.Name = name!;
                type.DefaultFrequency = frequency;
                type.IsActive = isActive;

                if (isNew) existing[code!] = type;
                Track(context, row, type, isNew);
            }
        }
    }
}