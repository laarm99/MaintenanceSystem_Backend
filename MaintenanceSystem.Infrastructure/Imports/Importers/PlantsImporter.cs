using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class PlantsImporter : EntityImporter
    {
        public override ImportType Type => ImportType.Plants;

        public override int Order => 2;

        public override IReadOnlyList<ImportType> DependsOn => [ImportType.Sites];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("SITE_CODE", true, "Código del sitio (debe existir).", "JRZ"),
            ImportColumn.Text("CODE", true, "Código de la planta, único por sitio (máx. 20).", "J1"),
            ImportColumn.Text("NAME", true, "Nombre de la planta (máx. 100).", "Planta J1"),
            ImportColumn.Boolean("IS_ACTIVE", "Activa. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var sites = await context.Db.Sites.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var existing = (await context.Db.Plants.Include(x => x.Site).ToListAsync(cancellationToken))
                .ToDictionary(x => PlantKey(x.Site.Code, x.Code), StringComparer.OrdinalIgnoreCase);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var siteCode = row.Code("SITE_CODE", 20, true);
                var code = row.Code("CODE", 20, true);
                var name = row.Text("NAME", 100, true);
                var isActive = row.Bool("IS_ACTIVE", true);

                Site? site = null;
                if (siteCode is not null && !sites.TryGetValue(siteCode, out site))
                    row.AddError($"SITE_CODE: el sitio '{siteCode}' no existe.");

                var key = PlantKey(siteCode ?? string.Empty, code ?? string.Empty);
                row.Key = key;
                if (siteCode is not null && code is not null) row.EnsureUnique(seen, key);
                if (row.HasErrors) continue;

                var isNew = !existing.TryGetValue(key, out var plant);
                plant ??= new Plant { SiteId = site!.Id, Code = code! };
                plant.Name = name!;
                plant.IsActive = isActive;

                if (isNew) existing[key] = plant;
                Track(context, row, plant, isNew);
            }
        }
    }
}