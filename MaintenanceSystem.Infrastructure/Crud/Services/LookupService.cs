using MaintenanceSystem.Application.Modules.Common;
using MaintenanceSystem.Application.Modules.Common.DTOs;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud.Services
{
    internal sealed class LookupService(ProjectDbContext db) : ILookupService
    {
        public async Task<IReadOnlyList<AccountLookupDTO>> GetActiveAccountsAsync(CancellationToken cancellationToken) =>
            await db.IDMAccounts
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.FullName)
                .Select(x => new AccountLookupDTO
                {
                    Id = x.Id,
                    DomainUserName = x.DomainUserName,
                    FullName = x.FullName
                })
                .ToListAsync(cancellationToken);
    }
}