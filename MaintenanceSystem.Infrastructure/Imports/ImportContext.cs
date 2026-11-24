using MaintenanceSystem.Infrastructure.Persistence;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal sealed record ImportContext(ProjectDbContext Db, string UserName);
}