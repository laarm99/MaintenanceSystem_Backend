using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class NonWorkingDayConfiguration : IEntityTypeConfiguration<NonWorkingDay>
    {
        public void Configure(EntityTypeBuilder<NonWorkingDay> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Date)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.HasIndex(x => new { x.SiteId, x.PlantId, x.Date })
                   .IsUnique();

            builder.HasOne(x => x.Site)
                   .WithMany(x => x.NonWorkingDays)
                   .HasForeignKey(x => x.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plant)
                   .WithMany()
                   .HasForeignKey(x => x.PlantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
