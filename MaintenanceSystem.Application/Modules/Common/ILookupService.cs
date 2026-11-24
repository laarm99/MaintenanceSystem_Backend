using MaintenanceSystem.Application.Modules.Common.DTOs;

namespace MaintenanceSystem.Application.Modules.Common
{
    public interface ILookupService
    {
        Task<IReadOnlyList<AccountLookupDTO>> GetActiveAccountsAsync(CancellationToken cancellationToken);
    }
}