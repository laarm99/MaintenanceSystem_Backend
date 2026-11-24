using MaintenanceSystem.Common.Results;

namespace MaintenanceSystem.Application.Modules.Common
{
    /// <summary>
    /// Standard list/get/create/update/delete operations used by the catalog and equipment screens.
    /// </summary>
    public interface ICrudService<TDto, TSaveRequest>
    {
        Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<ServiceResult<TDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult<TDto>> CreateAsync(TSaveRequest request, string userName, CancellationToken cancellationToken);

        Task<ServiceResult<TDto>> UpdateAsync(int id, TSaveRequest request, string userName, CancellationToken cancellationToken);

        Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}