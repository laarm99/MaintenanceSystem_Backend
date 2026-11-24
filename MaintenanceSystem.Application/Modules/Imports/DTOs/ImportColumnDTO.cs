namespace MaintenanceSystem.Application.Modules.Imports.DTOs
{
    public class ImportColumnDTO
    {
        public string Name { get; set; } = string.Empty;

        public bool Required { get; set; }

        public ImportColumnKind Kind { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? Example { get; set; }

        public IReadOnlyList<string>? AllowedValues { get; set; }
    }
}