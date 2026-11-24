using MaintenanceSystem.Domain.Entities.Maintenance;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Maintenance
{
    public class MaintenanceActivityConfiguration : IEntityTypeConfiguration<MaintenanceActivity>
    {
        public void Configure(EntityTypeBuilder<MaintenanceActivity> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.OriginalScheduledDate)
                   .IsRequired();

            builder.Property(x => x.ScheduledDate)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.IsUnplanned)
                   .IsRequired();

            builder.Property(x => x.TicketNumber)
                   .HasMaxLength(50);

            builder.Property(x => x.Notes)
                   .HasMaxLength(2000);

            builder.Property(x => x.NotPerformedReason)
                   .HasMaxLength(500);

            builder.HasIndex(x => x.ScheduledDate);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => new { x.EquipmentId, x.ScheduledDate });

            builder.HasOne(x => x.MaintenancePlan)
                   .WithMany(x => x.Activities)
                   .HasForeignKey(x => x.MaintenancePlanId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Equipment)
                   .WithMany(x => x.MaintenanceActivities)
                   .HasForeignKey(x => x.EquipmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TechnicianAccount)
                   .WithMany()
                   .HasForeignKey(x => x.TechnicianAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
