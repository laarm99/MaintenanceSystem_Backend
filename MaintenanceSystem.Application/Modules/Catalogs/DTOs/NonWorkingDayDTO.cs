using MaintenanceSystem.Domain.Enums;

namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class NonWorkingDayDTO
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string SiteCode { get; set; } = string.Empty;
        public int? PlantId { get; set; }
        public string? PlantCode { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public NonWorkingDayType Type { get; set; }
    }

    public class SaveNonWorkingDayRequest
    {
        public int SiteId { get; set; }
        public int? PlantId { get; set; }
        public DateOnly? Date { get; set; }
        public string? Description { get; set; }
        public NonWorkingDayType? Type { get; set; }
    }
}