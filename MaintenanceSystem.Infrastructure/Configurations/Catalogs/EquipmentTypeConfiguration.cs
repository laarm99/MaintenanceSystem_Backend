using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
    {
        public void Configure(EntityTypeBuilder<EquipmentType> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.DefaultFrequency)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasIndex(x => x.Code)
                   .IsUnique();
        }
    }
}
