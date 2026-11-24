using MaintenanceSystem.Application.Modules.Imports;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal interface IEntityImporter
    {
        ImportType Type { get; }

        /// <summary>Suggested load order; lower values must be loaded first.</summary>
        int Order { get; }

        IReadOnlyList<ImportType> DependsOn { get; }

        IReadOnlyList<ImportColumn> Columns { get; }

        /// <summary>
        /// Validates every row and adds/updates the tracked entities. Saving is done by the caller.
        /// </summary>
        Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken);
    }
}