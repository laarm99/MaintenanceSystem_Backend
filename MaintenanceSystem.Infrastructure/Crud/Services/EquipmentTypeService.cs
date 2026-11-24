using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class EquipmentTypeService(ProjectDbContext db)
        : CrudService<EquipmentType, EquipmentTypeDTO, SaveEquipmentTypeRequest>(db)
    {
        protected override string EntityLabel => "El tipo de equipo";

        protected override IQueryable<EquipmentType> Sort(IQueryable<EquipmentType> query) => query.OrderBy(x => x.Code);

        protected override EquipmentTypeDTO ToDto(EquipmentType x) => new()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            DefaultFrequency = x.DefaultFrequency,
            IsActive = x.IsActive
        };

        protected override void Normalize(SaveEquipmentTypeRequest r)
        {
            r.Code = Check.Code(r.Code);
            r.Name = Check.Clean(r.Name);
        }

        protected override async Task ValidateAsync(SaveEquipmentTypeRequest r, EquipmentType? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Code, "El código", 20, true);
            Check.Text(errors, r.Name, "El nombre", 100, true);

            var id = current?.Id ?? 0;
            if (r.Code is not null && await Db.EquipmentTypes.AnyAsync(x => x.Code == r.Code && x.Id != id, ct))
                errors.Add($"Ya existe un tipo de equipo con el código '{r.Code}'.");
        }

        protected override void Apply(SaveEquipmentTypeRequest r, EquipmentType e)
        {
            e.Code = r.Code!;
            e.Name = r.Name!;
            e.DefaultFrequency = r.DefaultFrequency;
            e.IsActive = r.IsActive;
        }
    }
}