using MaintenanceSystem.Domain.Entities.IDM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.IDM
{
    public class IDMAccountRoleConfiguration : IEntityTypeConfiguration<IDMAccountRole>
    {
        public void Configure(EntityTypeBuilder<IDMAccountRole> builder)
        {
            builder.HasKey(x => new { x.AccountId, x.RoleId });

            builder.HasOne(x => x.Account)
                   .WithMany()
                   .HasForeignKey(x => x.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                   .WithMany()
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
