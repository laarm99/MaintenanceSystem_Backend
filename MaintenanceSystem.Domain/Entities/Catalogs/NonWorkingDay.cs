using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatNonWorkingDay)]
    public class NonWorkingDay : AuditableEntity
    {
        [Column("SITE_ID")]
        public int SiteId { get; set; }

        // Null applies to every plant of the site.
        [Column("PLANT_ID")]
        public int? PlantId { get; set; }

        [Column("DATE")]
        public DateOnly Date { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;

        [Column("TYPE")]
        public NonWorkingDayType Type { get; set; }

        public Site Site { get; set; } = null!;

        public Plant? Plant { get; set; }
    }
}
