using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Common
{
    public abstract class AuditableEntity
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("CREATED_BY")]
        public string CreatedBy { get; set; } = string.Empty;

        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }

        [Column("UPDATED_BY")]
        public string? UpdatedBy { get; set; }
    }
}
