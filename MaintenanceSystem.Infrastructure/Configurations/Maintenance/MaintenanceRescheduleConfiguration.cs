using MaintenanceSystem.Domain.Entities.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Maintenance
{
    public class MaintenanceRescheduleConfiguration : IEntityTypeConfiguration<MaintenanceReschedule>
    {
        public void Configure(EntityTypeBuilder<MaintenanceReschedule> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.PreviousDate)
                   .IsRequired();

            builder.Property(x => x.NewDate)
                   .IsRequired();

            builder.Property(x => x.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.ChangedDate)
                   .IsRequired();

            builder.HasOne(x => x.MaintenanceActivity)
                   .WithMany(x => x.Reschedules)
                   .HasForeignKey(x => x.MaintenanceActivityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChangedByAccount)
                   .WithMany()
                   .HasForeignKey(x => x.ChangedByAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
