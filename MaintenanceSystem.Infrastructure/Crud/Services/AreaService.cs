using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class AreaService(ProjectDbContext db) : CrudService<Area, AreaDTO, SaveAreaRequest>(db)
    {
        protected override string EntityLabel => "El área";

        protected override IQueryable<Area> ReadQuery => Db.Areas.Include(x => x.Plant).ThenInclude(x => x.Site);

        protected override IQueryable<Area> Sort(IQueryable<Area> query) =>
            query.OrderBy(x => x.Plant.Site.Code).ThenBy(x => x.Plant.Code).ThenBy(x => x.Name);

        protected override AreaDTO ToDto(Area x) => new()
        {
            Id = x.Id,
            SiteId = x.Plant.SiteId,
            SiteCode = x.Plant.Site.Code,
            PlantId = x.PlantId,
            PlantCode = x.Plant.Code,
            PlantName = x.Plant.Name,
            Name = x.Name,
            IsActive = x.IsActive
        };

        protected override void Normalize(SaveAreaRequest r)
        {
            r.Name = Check.Clean(r.Name);
        }

        protected override async Task ValidateAsync(SaveAreaRequest r, Area? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Name, "El nombre", 100, true);

            if (!await Db.Plants.AnyAsync(x => x.Id == r.PlantId, ct))
            {
                errors.Add("La planta es obligatoria.");
                return;
            }

            var id = current?.Id ?? 0;
            if (r.Name is not null && await Db.Areas.AnyAsync(x => x.PlantId == r.PlantId && x.Name == r.Name && x.Id != id, ct))
                errors.Add($"Ya existe el área '{r.Name}' en la planta.");
        }

        protected override void Apply(SaveAreaRequest r, Area e)
        {
            e.PlantId = r.PlantId;
            e.Name = r.Name!;
            e.IsActive = r.IsActive;
        }
    }
}