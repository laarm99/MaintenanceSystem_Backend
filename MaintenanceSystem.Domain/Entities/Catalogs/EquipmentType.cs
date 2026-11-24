using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatEquipmentType)]
    public class EquipmentType : AuditableEntity
    {
        [Column("CODE")]
        public string Code { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("DEFAULT_FREQUENCY")]
        public MaintenanceFrequency? DefaultFrequency { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;
    }
}
