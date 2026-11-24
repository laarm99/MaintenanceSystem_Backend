using MaintenanceSystem.Domain.Entities.IDM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.IDM
{
    public class IDMRoleConfiguration : IEntityTypeConfiguration<IDMRole>
    {
        public void Configure(EntityTypeBuilder<IDMRole> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.RoleName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasIndex(x => x.RoleName)
                   .IsUnique();
        }
    }
}
