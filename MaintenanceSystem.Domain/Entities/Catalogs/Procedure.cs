using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatProcedure)]
    public class Procedure : AuditableEntity
    {
        [Column("CODE")]
        public string Code { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("EQUIPMENT_TYPE_ID")]
        public int? EquipmentTypeId { get; set; }

        [Column("ESTIMATED_DURATION_MINUTES")]
        public int EstimatedDurationMinutes { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        public EquipmentType? EquipmentType { get; set; }

        public ICollection<ProcedureChecklistItem> ChecklistItems { get; set; } = new List<ProcedureChecklistItem>();
    }
}
