using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class SiteService(ProjectDbContext db) : CrudService<Site, SiteDTO, SaveSiteRequest>(db)
    {
        public const string DefaultTimeZone = "America/Ciudad_Juarez";

        protected override string EntityLabel => "El sitio";

        protected override IQueryable<Site> Sort(IQueryable<Site> query) => query.OrderBy(x => x.Code);

        protected override SiteDTO ToDto(Site x) => new()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            TimeZone = x.TimeZone,
            IsActive = x.IsActive
        };

        protected override void Normalize(SaveSiteRequest r)
        {
            r.Code = Check.Code(r.Code);
            r.Name = Check.Clean(r.Name);
            r.TimeZone = Check.Clean(r.TimeZone) ?? DefaultTimeZone;
        }

        protected override async Task ValidateAsync(SaveSiteRequest r, Site? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Code, "El código", 20, true);
            Check.Text(errors, r.Name, "El nombre", 100, true);
            Check.Text(errors, r.TimeZone, "La zona horaria", 100, true);

            var id = current?.Id ?? 0;
            if (r.Code is not null && await Db.Sites.AnyAsync(x => x.Code == r.Code && x.Id != id, ct))
                errors.Add($"Ya existe un sitio con el código '{r.Code}'.");
        }

        protected override void Apply(SaveSiteRequest r, Site e)
        {
            e.Code = r.Code!;
            e.Name = r.Name!;
            e.TimeZone = r.TimeZone!;
            e.IsActive = r.IsActive;
        }
    }
}