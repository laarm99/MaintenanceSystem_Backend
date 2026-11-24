using MaintenanceSystem.Domain.Entities.IDM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.IDM
{
    public class IDMAccountConfiguration : IEntityTypeConfiguration<IDMAccount>
    {
        public void Configure(EntityTypeBuilder<IDMAccount> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.DomainUserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.HasIndex(x => x.DomainUserName)
                   .IsUnique();
        }
    }
}
