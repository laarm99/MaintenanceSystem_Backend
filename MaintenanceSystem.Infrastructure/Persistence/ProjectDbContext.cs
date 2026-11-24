using MaintenanceSystem.Domain.Entities.Audit;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Domain.Entities.Equipments;
using MaintenanceSystem.Domain.Entities.IDM;
using MaintenanceSystem.Domain.Entities.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        // IDM
        public DbSet<IDMAccount> IDMAccounts { get; set; }
        public DbSet<IDMRole> IDMRoles { get; set; }
        public DbSet<IDMAccountRole> IDMAccountRoles { get; set; }

        // Catalogs
        public DbSet<Site> Sites { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureChecklistItem> ProcedureChecklistItems { get; set; }
        public DbSet<NonWorkingDay> NonWorkingDays { get; set; }

        // Equipments
        public DbSet<Equipment> Equipments { get; set; }

        // Maintenance
        public DbSet<MaintenancePlan> MaintenancePlans { get; set; }
        public DbSet<MaintenanceActivity> MaintenanceActivities { get; set; }
        public DbSet<MaintenanceChecklistResult> MaintenanceChecklistResults { get; set; }
        public DbSet<MaintenanceEvidence> MaintenanceEvidences { get; set; }
        public DbSet<MaintenanceReschedule> MaintenanceReschedules { get; set; }

        // Audit
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetAuditDates();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditDates();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetAuditDates()
        {
            var now = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = now;
                    entry.Property(x => x.CreatedDate).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }
    }
}
