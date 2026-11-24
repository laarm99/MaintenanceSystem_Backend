using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Audit
{
    [Table(DbTables.AudAuditLog)]
    public class AuditLog
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("ENTITY_NAME")]
        public string EntityName { get; set; } = string.Empty;

        [Column("ENTITY_ID")]
        public string EntityId { get; set; } = string.Empty;

        [Column("ACTION")]
        public AuditAction Action { get; set; }

        // JSON with the changed values (old/new).
        [Column("CHANGES")]
        public string? Changes { get; set; }

        [Column("USER_NAME")]
        public string UserName { get; set; } = string.Empty;

        [Column("ACTION_DATE")]
        public DateTime ActionDate { get; set; }
    }
}
