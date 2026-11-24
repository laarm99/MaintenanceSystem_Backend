namespace MaintenanceSystem.Application.Modules.Common.DTOs
{
    public class AccountLookupDTO
    {
        public int Id { get; set; }
        public string DomainUserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}