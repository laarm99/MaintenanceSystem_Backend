using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Entities.Equipments;
using MaintenanceSystem.Domain.Entities.IDM;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Maintenance
{
    [Table(DbTables.MntActivity)]
    public class MaintenanceActivity : AuditableEntity
    {
        // Null when the activity was added outside the annual plan (urgent/unplanned).
        [Column("MAINTENANCE_PLAN_ID")]
        public int? MaintenancePlanId { get; set; }

        [Column("EQUIPMENT_ID")]
        public int EquipmentId { get; set; }

        [Column("TECHNICIAN_ACCOUNT_ID")]
        public int? TechnicianAccountId { get; set; }

        [Column("ORIGINAL_SCHEDULED_DATE")]
        public DateOnly OriginalScheduledDate { get; set; }

        [Column("SCHEDULED_DATE")]
        public DateOnly ScheduledDate { get; set; }

        [Column("STARTED_DATE")]
        public DateTime? StartedDate { get; set; }

        [Column("COMPLETED_DATE")]
        public DateTime? CompletedDate { get; set; }

        [Column("STATUS")]
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;

        [Column("IS_UNPLANNED")]
        public bool IsUnplanned { get; set; }

        [Column("TICKET_NUMBER")]
        public string? TicketNumber { get; set; }

        [Column("NOTES")]
        public string? Notes { get; set; }

        [Column("NOT_PERFORMED_REASON")]
        public string? NotPerformedReason { get; set; }

        public MaintenancePlan? MaintenancePlan { get; set; }

        public Equipment Equipment { get; set; } = null!;

        public IDMAccount? TechnicianAccount { get; set; }

        public ICollection<MaintenanceChecklistResult> ChecklistResults { get; set; } = new List<MaintenanceChecklistResult>();

        public ICollection<MaintenanceEvidence> Evidences { get; set; } = new List<MaintenanceEvidence>();

        public ICollection<MaintenanceReschedule> Reschedules { get; set; } = new List<MaintenanceReschedule>();
    }
}
