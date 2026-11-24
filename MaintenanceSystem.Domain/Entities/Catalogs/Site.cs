using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatSite)]
    public class Site : AuditableEntity
    {
        [Column("CODE")]
        public string Code { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("TIME_ZONE")]
        public string TimeZone { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        public ICollection<Plant> Plants { get; set; } = new List<Plant>();

        public ICollection<NonWorkingDay> NonWorkingDays { get; set; } = new List<NonWorkingDay>();
    }
}
