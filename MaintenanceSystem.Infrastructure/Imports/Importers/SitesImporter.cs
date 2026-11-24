using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class SitesImporter : EntityImporter
    {
        private const string DefaultTimeZone = "America/Ciudad_Juarez";

        public override ImportType Type => ImportType.Sites;

        public override int Order => 1;

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("CODE", true, "Código único del sitio (máx. 20).", "JRZ"),
            ImportColumn.Text("NAME", true, "Nombre del sitio (máx. 100).", "Campus Juárez"),
            ImportColumn.Text("TIME_ZONE", false, $"Zona horaria IANA. Si se deja vacío se usa {DefaultTimeZone}.", DefaultTimeZone),
            ImportColumn.Boolean("IS_ACTIVE", "Activo. Vacío = SI.", "SI")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var existing = await context.Db.Sites.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var code = row.Code("CODE", 20, true);
                var name = row.Text("NAME", 100, true);
                var timeZone = row.Text("TIME_ZONE", 100) ?? DefaultTimeZone;
                var isActive = row.Bool("IS_ACTIVE", true);

                row.Key = code ?? string.Empty;
                if (code is not null) row.EnsureUnique(seen, code);
                if (row.HasErrors) continue;

                var isNew = !existing.TryGetValue(code!, out var site);
                site ??= new Site { Code = code! };
                site.Name = name!;
                site.TimeZone = timeZone;
                site.IsActive = isActive;

                if (isNew) existing[code!] = site;
                Track(context, row, site, isNew);
            }
        }
    }
}