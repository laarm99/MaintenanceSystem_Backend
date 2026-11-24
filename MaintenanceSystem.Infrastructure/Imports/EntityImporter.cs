using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal abstract class EntityImporter : IEntityImporter
    {
        public abstract ImportType Type { get; }

        public abstract int Order { get; }

        public virtual IReadOnlyList<ImportType> DependsOn => [];

        public abstract IReadOnlyList<ImportColumn> Columns { get; }

        public abstract Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Call after the row values were assigned to <paramref name="entity"/>; sets audit user and the row action.
        /// </summary>
        protected static void Track<TEntity>(ImportContext context, ImportRow row, TEntity entity, bool isNew)
            where TEntity : class
        {
            if (isNew)
            {
                if (entity is AuditableEntity created) created.CreatedBy = context.UserName;
                context.Db.Add(entity);
                row.Action = ImportRowAction.Create;
                return;
            }

            var entry = context.Db.Entry(entity);
            entry.DetectChanges();

            if (entry.State == EntityState.Unchanged)
            {
                row.Action = ImportRowAction.Unchanged;
                return;
            }

            if (entity is AuditableEntity updated) updated.UpdatedBy = context.UserName;
            row.Action = ImportRowAction.Update;
        }

        protected static string PlantKey(string siteCode, string plantCode) => $"{siteCode}/{plantCode}";
    }
}