using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatArea)]
    public class Area : AuditableEntity
    {
        [Column("PLANT_ID")]
        public int PlantId { get; set; }

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        public Plant Plant { get; set; } = null!;
    }
}
