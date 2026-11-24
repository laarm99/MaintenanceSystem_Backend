using MaintenanceSystem.Application.Modules.Equipments.DTOs;
using MaintenanceSystem.Domain.Entities.Equipments;
using MaintenanceSystem.Domain.Enums;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class EquipmentService(ProjectDbContext db)
        : CrudService<Equipment, EquipmentDTO, SaveEquipmentRequest>(db)
    {
        protected override string EntityLabel => "El equipo";

        protected override string InUseMessage =>
            "El equipo tiene mantenimientos registrados y no se puede eliminar; cambie su estatus (Inactivo, Dado de baja, etc.).";

        protected override IQueryable<Equipment> ReadQuery =>
            Db.Equipments
                .Include(x => x.EquipmentType)
                .Include(x => x.Plant).ThenInclude(x => x.Site)
                .Include(x => x.Area)
                .Include(x => x.ResponsibleAccount)
                .Include(x => x.Procedure);

        protected override IQueryable<Equipment> Sort(IQueryable<Equipment> query) => query.OrderBy(x => x.Tag);

        protected override EquipmentDTO ToDto(Equipment x) => new()
        {
            Id = x.Id,
            Tag = x.Tag,
            Name = x.Name,
            EquipmentTypeId = x.EquipmentTypeId,
            EquipmentTypeName = x.EquipmentType.Name,
            Brand = x.Brand,
            Model = x.Model,
            SerialNumber = x.SerialNumber,
            SiteId = x.Plant.SiteId,
            SiteCode = x.Plant.Site.Code,
            PlantId = x.PlantId,
            PlantCode = x.Plant.Code,
            PlantName = x.Plant.Name,
            AreaId = x.AreaId,
            AreaName = x.Area?.Name,
            Location = x.Location,
            ResponsibleAccountId = x.ResponsibleAccountId,
            ResponsibleName = x.ResponsibleAccount?.FullName,
            ProcedureId = x.ProcedureId,
            ProcedureCode = x.Procedure?.Code,
            Frequency = x.Frequency,
            Criticality = x.Criticality,
            Status = x.Status,
            HasBackup = x.HasBackup,
            Notes = x.Notes
        };

        protected override void Normalize(SaveEquipmentRequest r)
        {
            r.Tag = Check.Code(r.Tag);
            r.Name = Check.Clean(r.Name);
            r.Brand = Check.Clean(r.Brand);
            r.Model = Check.Clean(r.Model);
            r.SerialNumber = Check.Clean(r.SerialNumber);
            r.Location = Check.Clean(r.Location);
            r.Notes = Check.Clean(r.Notes);
            r.Status ??= EquipmentStatus.Active;
        }

        protected override async Task ValidateAsync(SaveEquipmentRequest r, Equipment? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Tag, "El TAG", 50, true);
            Check.Text(errors, r.Name, "El nombre", 150, true);
            Check.Text(errors, r.Brand, "La marca", 100);
            Check.Text(errors, r.Model, "El modelo", 100);
            Check.Text(errors, r.SerialNumber, "El número de serie", 100);
            Check.Text(errors, r.Location, "La ubicación", 200);
            Check.Text(errors, r.Notes, "Las notas", 1000);
            Check.Required(errors, r.Criticality, "La criticidad");

            var type = await Db.EquipmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == r.EquipmentTypeId, ct);
            if (type is null)
                errors.Add("El tipo de equipo es obligatorio.");
            else if (r.Frequency is null)
            {
                if (type.DefaultFrequency is null)
                    errors.Add("La frecuencia es obligatoria (el tipo de equipo no tiene frecuencia por defecto).");
                else
                    r.Frequency = type.DefaultFrequency;
            }

            if (!await Db.Plants.AnyAsync(x => x.Id == r.PlantId, ct))
                errors.Add("La planta es obligatoria.");
            else if (r.AreaId is not null && !await Db.Areas.AnyAsync(x => x.Id == r.AreaId && x.PlantId == r.PlantId, ct))
                errors.Add("El área no pertenece a la planta seleccionada.");

            if (r.ResponsibleAccountId is not null && !await Db.IDMAccounts.AnyAsync(x => x.Id == r.ResponsibleAccountId, ct))
                errors.Add("El responsable no existe.");

            if (r.ProcedureId is not null && !await Db.Procedures.AnyAsync(x => x.Id == r.ProcedureId, ct))
                errors.Add("El procedimiento no existe.");

            var id = current?.Id ?? 0;
            if (r.Tag is not null && await Db.Equipments.AnyAsync(x => x.Tag == r.Tag && x.Id != id, ct))
                errors.Add($"Ya existe un equipo con el TAG '{r.Tag}'.");
        }

        protected override void Apply(SaveEquipmentRequest r, Equipment e)
        {
            e.Tag = r.Tag!;
            e.Name = r.Name!;
            e.EquipmentTypeId = r.EquipmentTypeId;
            e.Brand = r.Brand;
            e.Model = r.Model;
            e.SerialNumber = r.SerialNumber;
            e.PlantId = r.PlantId;
            e.AreaId = r.AreaId;
            e.Location = r.Location;
            e.ResponsibleAccountId = r.ResponsibleAccountId;
            e.ProcedureId = r.ProcedureId;
            e.Frequency = r.Frequency!.Value;
            e.Criticality = r.Criticality!.Value;
            e.Status = r.Status!.Value;
            e.HasBackup = r.HasBackup;
            e.Notes = r.Notes;
        }
    }
}