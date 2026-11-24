using MaintenanceSystem.Domain.Entities.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Maintenance
{
    public class MaintenanceChecklistResultConfiguration : IEntityTypeConfiguration<MaintenanceChecklistResult>
    {
        public void Configure(EntityTypeBuilder<MaintenanceChecklistResult> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(x => x.IsDone)
                   .IsRequired();

            builder.Property(x => x.Comments)
                   .HasMaxLength(500);

            builder.HasOne(x => x.MaintenanceActivity)
                   .WithMany(x => x.ChecklistResults)
                   .HasForeignKey(x => x.MaintenanceActivityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProcedureChecklistItem)
                   .WithMany()
                   .HasForeignKey(x => x.ProcedureChecklistItemId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
