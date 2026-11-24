using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class ProcedureService(ProjectDbContext db)
        : CrudService<Procedure, ProcedureDTO, SaveProcedureRequest>(db)
    {
        protected override string EntityLabel => "El procedimiento";

        protected override string InUseMessage =>
            "El procedimiento (o alguna actividad de su checklist) ya se usa en equipos o mantenimientos; desactívelo en su lugar.";

        protected override IQueryable<Procedure> ReadQuery =>
            Db.Procedures.Include(x => x.EquipmentType).Include(x => x.ChecklistItems);

        protected override IQueryable<Procedure> EditQuery => Db.Procedures.Include(x => x.ChecklistItems);

        protected override IQueryable<Procedure> Sort(IQueryable<Procedure> query) => query.OrderBy(x => x.Code);

        protected override ProcedureDTO ToDto(Procedure x) => new()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            EquipmentTypeId = x.EquipmentTypeId,
            EquipmentTypeName = x.EquipmentType?.Name,
            EstimatedDurationMinutes = x.EstimatedDurationMinutes,
            IsActive = x.IsActive,
            ChecklistItems = x.ChecklistItems
                .OrderBy(i => i.SortOrder)
                .Select(i => new ProcedureChecklistItemDTO
                {
                    Id = i.Id,
                    SortOrder = i.SortOrder,
                    Description = i.Description,
                    IsRequired = i.IsRequired,
                    IsActive = i.IsActive
                })
                .ToList()
        };

        protected override void Normalize(SaveProcedureRequest r)
        {
            r.Code = Check.Code(r.Code);
            r.Name = Check.Clean(r.Name);
            r.Description = Check.Clean(r.Description);
            r.ChecklistItems ??= [];
            r.ChecklistItems.RemoveAll(x => x is null);
            foreach (var item in r.ChecklistItems)
                item.Description = Check.Clean(item.Description);
        }

        protected override async Task ValidateAsync(SaveProcedureRequest r, Procedure? current, List<string> errors, CancellationToken ct)
        {
            Check.Text(errors, r.Code, "El código", 30, true);
            Check.Text(errors, r.Name, "El nombre", 150, true);
            Check.Text(errors, r.Description, "La descripción", 1000);

            if (r.EstimatedDurationMinutes is < 0 or > 1440)
                errors.Add("La duración estimada debe estar entre 0 y 1440 minutos.");

            if (r.EquipmentTypeId is not null && !await Db.EquipmentTypes.AnyAsync(x => x.Id == r.EquipmentTypeId, ct))
                errors.Add("El tipo de equipo no existe.");

            for (var i = 0; i < r.ChecklistItems.Count; i++)
            {
                var item = r.ChecklistItems[i];
                Check.Text(errors, item.Description, $"La actividad {i + 1} del checklist", 300, true);

                if (item.Id is not null && current?.ChecklistItems.All(x => x.Id != item.Id) != false)
                    errors.Add($"La actividad {i + 1} del checklist no pertenece al procedimiento.");
            }

            var id = current?.Id ?? 0;
            if (r.Code is not null && await Db.Procedures.AnyAsync(x => x.Code == r.Code && x.Id != id, ct))
                errors.Add($"Ya existe un procedimiento con el código '{r.Code}'.");
        }

        protected override void Apply(SaveProcedureRequest r, Procedure e)
        {
            e.Code = r.Code!;
            e.Name = r.Name!;
            e.Description = r.Description;
            e.EquipmentTypeId = r.EquipmentTypeId;
            e.EstimatedDurationMinutes = r.EstimatedDurationMinutes;
            e.IsActive = r.IsActive;

            var keepIds = r.ChecklistItems.Where(x => x.Id is not null).Select(x => x.Id!.Value).ToHashSet();
            foreach (var removed in e.ChecklistItems.Where(x => !keepIds.Contains(x.Id)).ToList())
                Db.ProcedureChecklistItems.Remove(removed);

            for (var i = 0; i < r.ChecklistItems.Count; i++)
            {
                var source = r.ChecklistItems[i];
                var item = source.Id is null
                    ? null
                    : e.ChecklistItems.First(x => x.Id == source.Id);

                if (item is null)
                {
                    item = new ProcedureChecklistItem();
                    e.ChecklistItems.Add(item);
                }

                item.SortOrder = i + 1;
                item.Description = source.Description!;
                item.IsRequired = source.IsRequired;
                item.IsActive = source.IsActive;
            }
        }
    }
}