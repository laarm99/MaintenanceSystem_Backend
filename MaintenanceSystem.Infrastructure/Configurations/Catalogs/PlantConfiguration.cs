using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasIndex(x => new { x.SiteId, x.Code })
                   .IsUnique();

            builder.HasOne(x => x.Site)
                   .WithMany(x => x.Plants)
                   .HasForeignKey(x => x.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
