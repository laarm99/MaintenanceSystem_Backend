using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class PlantService(ProjectDbContext db) : CrudService<Plant, PlantDTO, SavePlantRequest>(db)
    {
        protected override string EntityLabel => "La planta";

        protected override IQueryable<Plant> ReadQuery => Db.Plants.Include(x => x.Site);

        protected override IQueryable<Plant> Sort(IQueryable<Plant> query) =>
            query.OrderBy(x => x.Site.Code).ThenBy(x => x.Code);

        protected override PlantDTO ToDto(Plant x) => new()
        {
            Id = x.Id,
            SiteId = x.SiteId,
            SiteCode = x.Site.Code,
            SiteName = x.Site.Name,
            Code = x.Code,
            Name = x.Name,
            IsActive = x.IsActive
        };

        protected override void Normalize(SavePlantRequest r)
        {
            r.Code = Check.Code(r.Code);
            r.Name = Check.Clean(r.Name);
        }

        protected override async Task ValidateAsync(SavePlantRequest r, Plant? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Code, "El código", 20, true);
            Check.Text(errors, r.Name, "El nombre", 100, true);

            if (!await Db.Sites.AnyAsync(x => x.Id == r.SiteId, ct))
            {
                errors.Add("El sitio es obligatorio.");
                return;
            }

            var id = current?.Id ?? 0;
            if (r.Code is not null && await Db.Plants.AnyAsync(x => x.SiteId == r.SiteId && x.Code == r.Code && x.Id != id, ct))
                errors.Add($"Ya existe una planta con el código '{r.Code}' en el sitio.");
        }

        protected override void Apply(SavePlantRequest r, Plant e)
        {
            e.SiteId = r.SiteId;
            e.Code = r.Code!;
            e.Name = r.Name!;
            e.IsActive = r.IsActive;
        }
    }
}