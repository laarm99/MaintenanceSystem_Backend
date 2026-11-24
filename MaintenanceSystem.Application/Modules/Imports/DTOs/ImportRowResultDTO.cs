namespace MaintenanceSystem.Application.Modules.Imports.DTOs
{
    public class ImportRowResultDTO
    {
        public int RowNumber { get; set; }

        public string Key { get; set; } = string.Empty;

        public ImportRowAction Action { get; set; }

        public List<string> Errors { get; set; } = [];
    }
}