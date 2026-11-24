using MaintenanceSystem.Domain.Enums;

namespace MaintenanceSystem.Application.Modules.Equipments.DTOs
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public int SiteId { get; set; }
        public string SiteCode { get; set; } = string.Empty;
        public int PlantId { get; set; }
        public string PlantCode { get; set; } = string.Empty;
        public string PlantName { get; set; } = string.Empty;
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public string? Location { get; set; }
        public int? ResponsibleAccountId { get; set; }
        public string? ResponsibleName { get; set; }
        public int? ProcedureId { get; set; }
        public string? ProcedureCode { get; set; }
        public MaintenanceFrequency Frequency { get; set; }
        public CriticalityLevel Criticality { get; set; }
        public EquipmentStatus Status { get; set; }
        public bool HasBackup { get; set; }
        public string? Notes { get; set; }
    }

    public class SaveEquipmentRequest
    {
        public string? Tag { get; set; }
        public string? Name { get; set; }
        public int EquipmentTypeId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public int PlantId { get; set; }
        public int? AreaId { get; set; }
        public string? Location { get; set; }
        public int? ResponsibleAccountId { get; set; }
        public int? ProcedureId { get; set; }

        /// <summary>
        /// Empty = equipment type default frequency.
        /// </summary>
        public MaintenanceFrequency? Frequency { get; set; }
        public CriticalityLevel? Criticality { get; set; }
        public EquipmentStatus? Status { get; set; }
        public bool HasBackup { get; set; }
        public string? Notes { get; set; }
    }
}