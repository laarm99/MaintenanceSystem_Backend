using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class ProcedureConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.EstimatedDurationMinutes)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.HasOne(x => x.EquipmentType)
                   .WithMany()
                   .HasForeignKey(x => x.EquipmentTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
