using MaintenanceSystem.Application.Modules.Imports.DTOs;
using MaintenanceSystem.Common.Results;

namespace MaintenanceSystem.Application.Modules.Imports
{
    public interface IImportService
    {
        IReadOnlyList<ImportDefinitionDTO> GetDefinitions();

        ServiceResult<byte[]> BuildTemplate(ImportType type);

        /// <summary>
        /// Validates the file and, when <paramref name="commit"/> is true and no row has errors, saves all rows in one transaction.
        /// </summary>
        Task<ServiceResult<ImportResultDTO>> ImportAsync(
            ImportType type,
            Stream file,
            string fileName,
            bool commit,
            string userName,
            CancellationToken cancellationToken);
    }
}