using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class AreasImporter : EntityImporter
    {
        public override ImportType Type => ImportType.Areas;

        public override int Order => 3;

        public override IReadOnlyList<ImportType> DependsOn => [ImportType.Plants];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("SITE_CODE", true, "Código del sitio.", "JRZ"),
            ImportColumn.Text("PLANT_CODE", true, "Código de la planta (debe existir en el sitio).", "J1"),
            ImportColumn.Text("NAME", true, "Nombre del área, único por planta (máx. 100).", "Producción"),
            ImportColumn.Boolean("IS_ACTIVE", "Activa. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var plants = (await context.Db.Plants.Include(x => x.Site).ToListAsync(cancellationToken))
                .ToDictionary(x => PlantKey(x.Site.Code, x.Code), StringComparer.OrdinalIgnoreCase);
            var existing = (await context.Db.Areas.ToListAsync(cancellationToken))
                .ToDictionary(x => $"{x.PlantId}|{x.Name}", StringComparer.OrdinalIgnoreCase);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var siteCode = row.Code("SITE_CODE", 20, true);
                var plantCode = row.Code("PLANT_CODE", 20, true);
                var name = row.Text("NAME", 100, true);
                var isActive = row.Bool("IS_ACTIVE", true);

                Plant? plant = null;
                if (siteCode is not null && plantCode is not null && !plants.TryGetValue(PlantKey(siteCode, plantCode), out plant))
                    row.AddError($"PLANT_CODE: la planta '{plantCode}' no existe en el sitio '{siteCode}'.");

                row.Key = $"{PlantKey(siteCode ?? string.Empty, plantCode ?? string.Empty)}/{name}";
                if (plant is not null && name is not null) row.EnsureUnique(seen, row.Key);
                if (row.HasErrors) continue;

                var key = $"{plant!.Id}|{name}";
                var isNew = !existing.TryGetValue(key, out var area);
                area ??= new Area { PlantId = plant.Id };
                area.Name = name!;
                area.IsActive = isActive;

                if (isNew) existing[key] = area;
                Track(context, row, area, isNew);
            }
        }
    }
}