using MaintenanceSystem.Domain.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Catalogs
{
    public class ProcedureChecklistItemConfiguration : IEntityTypeConfiguration<ProcedureChecklistItem>
    {
        public void Configure(EntityTypeBuilder<ProcedureChecklistItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(x => x.SortOrder)
                   .IsRequired();

            builder.HasOne(x => x.Procedure)
                   .WithMany(x => x.ChecklistItems)
                   .HasForeignKey(x => x.ProcedureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
