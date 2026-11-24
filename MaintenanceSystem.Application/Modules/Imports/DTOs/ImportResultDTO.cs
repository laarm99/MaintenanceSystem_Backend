namespace MaintenanceSystem.Application.Modules.Imports.DTOs
{
    public class ImportResultDTO
    {
        public ImportType Type { get; set; }

        public string FileName { get; set; } = string.Empty;

        public bool Committed { get; set; }

        public int TotalRows { get; set; }

        public int CreatedCount { get; set; }

        public int UpdatedCount { get; set; }

        public int UnchangedCount { get; set; }

        public int ErrorCount { get; set; }

        public List<ImportRowResultDTO> Rows { get; set; } = [];
    }
}