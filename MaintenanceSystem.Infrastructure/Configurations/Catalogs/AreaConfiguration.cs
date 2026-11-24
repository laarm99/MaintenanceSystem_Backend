using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasIndex(x => new { x.PlantId, x.Name })
                   .IsUnique();

            builder.HasOne(x => x.Plant)
                   .WithMany(x => x.Areas)
                   .HasForeignKey(x => x.PlantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
