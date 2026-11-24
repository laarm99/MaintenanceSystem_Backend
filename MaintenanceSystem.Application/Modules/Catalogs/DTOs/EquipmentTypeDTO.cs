using MaintenanceSystem.Domain.Enums;

namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class EquipmentTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MaintenanceFrequency? DefaultFrequency { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaveEquipmentTypeRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public MaintenanceFrequency? DefaultFrequency { get; set; }
        public bool IsActive { get; set; } = true;
    }
}