using MaintenanceSystem.Domain.Entities.Maintenance;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Maintenance
{
    public class MaintenancePlanConfiguration : IEntityTypeConfiguration<MaintenancePlan>
    {
        public void Configure(EntityTypeBuilder<MaintenancePlan> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Year)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.PublishedBy)
                   .HasMaxLength(50);

            builder.Property(x => x.Notes)
                   .HasMaxLength(1000);

            builder.HasIndex(x => new { x.SiteId, x.Year })
                   .IsUnique();

            builder.HasOne(x => x.Site)
                   .WithMany()
                   .HasForeignKey(x => x.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
