using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Domain.Entities.IDM;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceSystem.Domain.Entities.Maintenance
{
    [Table(DbTables.MntEvidence)]
    public class MaintenanceEvidence
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("MAINTENANCE_ACTIVITY_ID")]
        public int MaintenanceActivityId { get; set; }

        [Column("FILE_NAME")]
        public string FileName { get; set; } = string.Empty;

        [Column("STORED_FILE_PATH")]
        public string StoredFilePath { get; set; } = string.Empty;

        [Column("CONTENT_TYPE")]
        public string ContentType { get; set; } = string.Empty;

        [Column("FILE_SIZE_BYTES")]
        public long FileSizeBytes { get; set; }

        [Column("UPLOADED_BY_ACCOUNT_ID")]
        public int UploadedByAccountId { get; set; }

        [Column("UPLOADED_DATE")]
        public DateTime UploadedDate { get; set; }

        public MaintenanceActivity MaintenanceActivity { get; set; } = null!;

        public IDMAccount UploadedByAccount { get; set; } = null!;
    }
}
