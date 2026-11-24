namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class AreaDTO
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string SiteCode { get; set; } = string.Empty;
        public int PlantId { get; set; }
        public string PlantCode { get; set; } = string.Empty;
        public string PlantName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SaveAreaRequest
    {
        public int PlantId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}