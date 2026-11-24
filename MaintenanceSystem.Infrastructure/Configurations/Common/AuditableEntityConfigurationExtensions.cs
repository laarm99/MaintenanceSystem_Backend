using MaintenanceSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Common
{
    public static class AuditableEntityConfigurationExtensions
    {
        public static void ConfigureAuditable<T>(this EntityTypeBuilder<T> builder) where T : AuditableEntity
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.UpdatedBy)
                   .HasMaxLength(50);
        }
    }
}
