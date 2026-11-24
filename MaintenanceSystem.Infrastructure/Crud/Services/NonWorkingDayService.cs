using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class NonWorkingDayService(ProjectDbContext db)
        : CrudService<NonWorkingDay, NonWorkingDayDTO, SaveNonWorkingDayRequest>(db)
    {
        protected override string EntityLabel => "El día inhábil";

        protected override IQueryable<NonWorkingDay> ReadQuery =>
            Db.NonWorkingDays.Include(x => x.Site).Include(x => x.Plant);

        protected override IQueryable<NonWorkingDay> Sort(IQueryable<NonWorkingDay> query) =>
            query.OrderByDescending(x => x.Date).ThenBy(x => x.Site.Code);

        protected override NonWorkingDayDTO ToDto(NonWorkingDay x) => new()
        {
            Id = x.Id,
            SiteId = x.SiteId,
            SiteCode = x.Site.Code,
            PlantId = x.PlantId,
            PlantCode = x.Plant?.Code,
            Date = x.Date,
            Description = x.Description,
            Type = x.Type
        };

        protected override void Normalize(SaveNonWorkingDayRequest r)
        {
            r.Description = Check.Clean(r.Description);
            r.Type ??= NonWorkingDayType.Holiday;
        }

        protected override async Task ValidateAsync(SaveNonWorkingDayRequest r, NonWorkingDay? current, List<string> errors, CancellationToken ct)
        {
            Check.Required(errors, r.Date, "La fecha");
            Check.Text(errors, r.Description, "La descripción", 200, true);

            if (!await Db.Sites.AnyAsync(x => x.Id == r.SiteId, ct))
            {
                errors.Add("El sitio es obligatorio.");
                return;
            }

            if (r.PlantId is not null && !await Db.Plants.AnyAsync(x => x.Id == r.PlantId && x.SiteId == r.SiteId, ct))
                errors.Add("La planta no pertenece al sitio seleccionado.");

            var id = current?.Id ?? 0;
            if (r.Date is not null &&
                await Db.NonWorkingDays.AnyAsync(x => x.SiteId == r.SiteId && x.PlantId == r.PlantId && x.Date == r.Date && x.Id != id, ct))
                errors.Add($"Ya existe un día inhábil para el {r.Date:yyyy-MM-dd} en ese sitio/planta.");
        }

        protected override void Apply(SaveNonWorkingDayRequest r, NonWorkingDay e)
        {
            e.SiteId = r.SiteId;
            e.PlantId = r.PlantId;
            e.Date = r.Date!.Value;
            e.Description = r.Description!;
            e.Type = r.Type!.Value;
        }
    }
}