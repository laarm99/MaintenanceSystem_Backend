using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Maintenance
{
    [Table(DbTables.MntPlan)]
    public class MaintenancePlan : AuditableEntity
    {
        [Column("SITE_ID")]
        public int SiteId { get; set; }

        [Column("YEAR")]
        public int Year { get; set; }

        [Column("STATUS")]
        public PlanStatus Status { get; set; } = PlanStatus.Draft;

        [Column("PUBLISHED_DATE")]
        public DateTime? PublishedDate { get; set; }

        [Column("PUBLISHED_BY")]
        public string? PublishedBy { get; set; }

        [Column("NOTES")]
        public string? Notes { get; set; }

        public Site Site { get; set; } = null!;

        public ICollection<MaintenanceActivity> Activities { get; set; } = new List<MaintenanceActivity>();
    }
}
