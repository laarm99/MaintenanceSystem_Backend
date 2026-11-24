using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class NonWorkingDaysImporter : EntityImporter
    {
        public override ImportType Type => ImportType.NonWorkingDays;

        public override int Order => 7;

        public override IReadOnlyList<ImportType> DependsOn => [ImportType.Sites, ImportType.Plants];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("SITE_CODE", true, "Código del sitio (debe existir).", "JRZ"),
            ImportColumn.Text("PLANT_CODE", false, "Código de la planta. Vacío = aplica a todo el sitio.", "J1"),
            ImportColumn.Date("DATE", true, "Fecha (AAAA-MM-DD).", "2027-01-01"),
            ImportColumn.Text("DESCRIPTION", true, "Descripción (máx. 200).", "Año nuevo"),
            ImportColumn.Enum<NonWorkingDayType>("TYPE", false, "Tipo de día. Vacío = Holiday.", nameof(NonWorkingDayType.Holiday))
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var sites = await context.Db.Sites.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var plants = (await context.Db.Plants.Include(x => x.Site).ToListAsync(cancellationToken))
                .ToDictionary(x => PlantKey(x.Site.Code, x.Code), StringComparer.OrdinalIgnoreCase);
            var existing = (await context.Db.NonWorkingDays.ToListAsync(cancellationToken))
                .ToDictionary(x => DayKey(x.SiteId, x.PlantId, x.Date));
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var siteCode = row.Code("SITE_CODE", 20, true);
                var plantCode = row.Code("PLANT_CODE", 20);
                var date = row.Date("DATE", true);
                var description = row.Text("DESCRIPTION", 200, true);
                var type = row.Enum<NonWorkingDayType>("TYPE", false) ?? NonWorkingDayType.Holiday;

                Site? site = null;
                Plant? plant = null;
                if (siteCode is not null && !sites.TryGetValue(siteCode, out site))
                    row.AddError($"SITE_CODE: el sitio '{siteCode}' no existe.");
                else if (siteCode is not null && plantCode is not null && !plants.TryGetValue(PlantKey(siteCode, plantCode), out plant))
                    row.AddError($"PLANT_CODE: la planta '{plantCode}' no existe en el sitio '{siteCode}'.");

                row.Key = $"{siteCode}/{plantCode ?? "*"}/{date:yyyy-MM-dd}";
                if (siteCode is not null && date is not null) row.EnsureUnique(seen, row.Key);
                if (row.HasErrors) continue;

                var key = DayKey(site!.Id, plant?.Id, date!.Value);
                var isNew = !existing.TryGetValue(key, out var day);
                day ??= new NonWorkingDay { SiteId = site.Id, PlantId = plant?.Id, Date = date.Value };
                day.Description = description!;
                day.Type = type;

                if (isNew) existing[key] = day;
                Track(context, row, day, isNew);
            }
        }

        private static string DayKey(int siteId, int? plantId, DateOnly date) =>
            $"{siteId}|{plantId?.ToString() ?? "*"}|{date:yyyy-MM-dd}";
    }
}