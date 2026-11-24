namespace MaintenanceSystem.Application.Modules.Catalogs.DTOs
{
    public class ProcedureDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? EquipmentTypeId { get; set; }
        public string? EquipmentTypeName { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public bool IsActive { get; set; }
        public List<ProcedureChecklistItemDTO> ChecklistItems { get; set; } = [];
    }

    public class ProcedureChecklistItemDTO
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaveProcedureRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? EquipmentTypeId { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Full checklist in display order. Items with Id are updated, without Id are created, missing ones are deleted.
        /// </summary>
        public List<SaveProcedureChecklistItemRequest> ChecklistItems { get; set; } = [];
    }

    public class SaveProcedureChecklistItemRequest
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public bool IsRequired { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}