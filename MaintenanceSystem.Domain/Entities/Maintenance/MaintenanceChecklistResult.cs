using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Catalogs;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Maintenance
{
    [Table(DbTables.MntChecklistResult)]
    public class MaintenanceChecklistResult
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("MAINTENANCE_ACTIVITY_ID")]
        public int MaintenanceActivityId { get; set; }

        [Column("PROCEDURE_CHECKLIST_ITEM_ID")]
        public int? ProcedureChecklistItemId { get; set; }

        // Snapshot of the item text so history survives catalog changes.
        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;

        [Column("IS_DONE")]
        public bool IsDone { get; set; }

        [Column("COMMENTS")]
        public string? Comments { get; set; }

        public MaintenanceActivity MaintenanceActivity { get; set; } = null!;

        public ProcedureChecklistItem? ProcedureChecklistItem { get; set; }
    }
}
