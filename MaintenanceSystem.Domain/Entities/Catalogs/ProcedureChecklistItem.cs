using MaintenanceSystem.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Catalogs
{
    [Table(DbTables.CatProcedureChecklistItem)]
    public class ProcedureChecklistItem
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("PROCEDURE_ID")]
        public int ProcedureId { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;

        [Column("SORT_ORDER")]
        public int SortOrder { get; set; }

        [Column("IS_REQUIRED")]
        public bool IsRequired { get; set; } = true;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        public Procedure Procedure { get; set; } = null!;
    }
}
