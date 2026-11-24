using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.IDM;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Maintenance
{
    [Table(DbTables.MntReschedule)]
    public class MaintenanceReschedule
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("MAINTENANCE_ACTIVITY_ID")]
        public int MaintenanceActivityId { get; set; }

        [Column("PREVIOUS_DATE")]
        public DateOnly PreviousDate { get; set; }

        [Column("NEW_DATE")]
        public DateOnly NewDate { get; set; }

        [Column("REASON")]
        public string Reason { get; set; } = string.Empty;

        [Column("CHANGED_BY_ACCOUNT_ID")]
        public int ChangedByAccountId { get; set; }

        [Column("CHANGED_DATE")]
        public DateTime ChangedDate { get; set; }

        public MaintenanceActivity MaintenanceActivity { get; set; } = null!;

        public IDMAccount ChangedByAccount { get; set; } = null!;
    }
}
