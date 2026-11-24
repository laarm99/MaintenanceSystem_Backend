namespace MaintenanceSystem.Application.Modules.Imports.DTOs
{
    public class ImportDefinitionDTO
    {
        public ImportType Type { get; set; }

        public int Order { get; set; }

        public IReadOnlyList<ImportType> DependsOn { get; set; } = [];

        public IReadOnlyList<ImportColumnDTO> Columns { get; set; } = [];
    }
}