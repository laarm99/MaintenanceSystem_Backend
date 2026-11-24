using MaintenanceSystem.Domain.Entities.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Maintenance
{
    public class MaintenanceEvidenceConfiguration : IEntityTypeConfiguration<MaintenanceEvidence>
    {
        public void Configure(EntityTypeBuilder<MaintenanceEvidence> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.StoredFilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.FileSizeBytes)
                   .IsRequired();

            builder.Property(x => x.UploadedDate)
                   .IsRequired();

            builder.HasOne(x => x.MaintenanceActivity)
                   .WithMany(x => x.Evidences)
                   .HasForeignKey(x => x.MaintenanceActivityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.UploadedByAccount)
                   .WithMany()
                   .HasForeignKey(x => x.UploadedByAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
