namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class PlantDTO
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string SiteCode { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SavePlantRequest
    {
        public int SiteId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}