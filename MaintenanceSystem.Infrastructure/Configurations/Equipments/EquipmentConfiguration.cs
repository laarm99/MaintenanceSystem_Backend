using MaintenanceSystem.Domain.Entities.Equipments;
using MaintenanceSystem.Infrastructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceSystem.Infrastructure.Configurations.Equipments
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ConfigureAuditable();

            builder.Property(x => x.Tag)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Brand)
                   .HasMaxLength(100);

            builder.Property(x => x.Model)
                   .HasMaxLength(100);

            builder.Property(x => x.SerialNumber)
                   .HasMaxLength(100);

            builder.Property(x => x.Location)
                   .HasMaxLength(200);

            builder.Property(x => x.Frequency)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.Criticality)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.HasBackup)
                   .IsRequired();

            builder.Property(x => x.Notes)
                   .HasMaxLength(1000);

            builder.HasIndex(x => x.Tag)
                   .IsUnique();

            builder.HasIndex(x => x.SerialNumber);

            builder.HasOne(x => x.EquipmentType)
                   .WithMany()
                   .HasForeignKey(x => x.EquipmentTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plant)
                   .WithMany()
                   .HasForeignKey(x => x.PlantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Area)
                   .WithMany()
                   .HasForeignKey(x => x.AreaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ResponsibleAccount)
                   .WithMany()
                   .HasForeignKey(x => x.ResponsibleAccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Procedure)
                   .WithMany()
                   .HasForeignKey(x => x.ProcedureId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
