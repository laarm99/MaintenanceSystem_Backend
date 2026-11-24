namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class SiteDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SaveSiteRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? TimeZone { get; set; }
        public bool IsActive { get; set; } = true;
    }
}