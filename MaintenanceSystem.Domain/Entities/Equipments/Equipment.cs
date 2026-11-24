using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Entities.IDM;
using MaintenanceSystem.Domain.Entities.Maintenance;
using MaintenanceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Equipments
{
    [Table(DbTables.MntEquipment)]
    public class Equipment : AuditableEntity
    {
        [Column("TAG")]
        public string Tag { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [Column("EQUIPMENT_TYPE_ID")]
        public int EquipmentTypeId { get; set; }

        [Column("BRAND")]
        public string? Brand { get; set; }

        [Column("MODEL")]
        public string? Model { get; set; }

        [Column("SERIAL_NUMBER")]
        public string? SerialNumber { get; set; }

        [Column("PLANT_ID")]
        public int PlantId { get; set; }

        [Column("AREA_ID")]
        public int? AreaId { get; set; }

        [Column("LOCATION")]
        public string? Location { get; set; }

        [Column("RESPONSIBLE_ACCOUNT_ID")]
        public int? ResponsibleAccountId { get; set; }

        [Column("PROCEDURE_ID")]
        public int? ProcedureId { get; set; }

        [Column("FREQUENCY")]
        public MaintenanceFrequency Frequency { get; set; }

        [Column("CRITICALITY")]
        public CriticalityLevel Criticality { get; set; }

        [Column("STATUS")]
        public EquipmentStatus Status { get; set; } = EquipmentStatus.Active;

        [Column("HAS_BACKUP")]
        public bool HasBackup { get; set; }

        [Column("NOTES")]
        public string? Notes { get; set; }

        public EquipmentType EquipmentType { get; set; } = null!;

        public Plant Plant { get; set; } = null!;

        public Area? Area { get; set; }

        public IDMAccount? ResponsibleAccount { get; set; }

        public Procedure? Procedure { get; set; }

        public ICollection<MaintenanceActivity> MaintenanceActivities { get; set; } = new List<MaintenanceActivity>();
    }
}
