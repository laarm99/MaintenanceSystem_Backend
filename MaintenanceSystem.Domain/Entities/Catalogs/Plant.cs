using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatPlant)]
    public class Plant : AuditableEntity
    {
        [Column("SITE_ID")]
        public int SiteId { get; set; }

        [Column("CODE")]
        public string Code { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        public Site Site { get; set; } = null!;

        public ICollection<Area> Areas { get; set; } = new List<Area>();
    }
}
