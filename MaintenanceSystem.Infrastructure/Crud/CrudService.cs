using MaintenanceSystem.Application.Modules.Common;
using MaintenanceSystem.Common.Results;
using MaintenanceSystem.Domain.Entities.Common;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Crud
{
    /// <summary>
    /// Base CRUD flow: normalize request -> validate -> apply to entity -> save -> return the refreshed DTO.
    /// </summary>
    internal abstract class CrudService<TEntity, TDto, TSaveRequest> : ICrudService<TDto, TSaveRequest>
        where TEntity : AuditableEntity, new()
        where TSaveRequest : class
    {
        protected CrudService(ProjectDbContext db)
        {
            Db = db;
        }

        protected ProjectDbContext Db { get; }

        /// <summary>Spanish label used in messages, e.g. "El sitio".</summary>
        protected abstract string EntityLabel { get; }

        protected virtual string InUseMessage =>
            $"{EntityLabel} está en uso por otros registros y no se puede eliminar; desactívelo en su lugar.";

        /// <summary>Query with the navigations needed by <see cref="ToDto"/>.</summary>
        protected virtual IQueryable<TEntity> ReadQuery => Db.Set<TEntity>();

        /// <summary>Tracked query used for update/delete (include owned children here).</summary>
        protected virtual IQueryable<TEntity> EditQuery => Db.Set<TEntity>();

        protected virtual IQueryable<TEntity> Sort(IQueryable<TEntity> query) => query.OrderBy(x => x.Id);

        protected abstract TDto ToDto(TEntity entity);

        /// <summary>Trim/upper-case values before validating.</summary>
        protected virtual void Normalize(TSaveRequest request) { }

        protected abstract Task ValidateAsync(TSaveRequest request, TEntity? current, List<string> errors, CancellationToken cancellationToken);

        protected abstract void Apply(TSaveRequest request, TEntity entity);

        public async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await Sort(ReadQuery.AsNoTracking()).ToListAsync(cancellationToken);
            return entities.Select(ToDto).ToList();
        }

        public async Task<ServiceResult<TDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await ReadQuery.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity is null
                ? ServiceResult<TDto>.NotFound($"{EntityLabel} no existe.")
                : ServiceResult<TDto>.Success(ToDto(entity));
        }

        public Task<ServiceResult<TDto>> CreateAsync(TSaveRequest request, string userName, CancellationToken cancellationToken) =>
            SaveAsync(null, request, userName, cancellationToken);

        public Task<ServiceResult<TDto>> UpdateAsync(int id, TSaveRequest request, string userName, CancellationToken cancellationToken) =>
            SaveAsync(id, request, userName, cancellationToken);

        public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await EditQuery.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
                return ServiceResult<bool>.NotFound($"{EntityLabel} no existe.");

            Db.Remove(entity);
            try
            {
                await Db.SaveChangesAsync(cancellationToken);
                return ServiceResult<bool>.Success(true);
            }
            catch (DbUpdateException)
            {
                return ServiceResult<bool>.Conflict(InUseMessage);
            }
            finally
            {
                Db.ChangeTracker.Clear();
            }
        }

        private async Task<ServiceResult<TDto>> SaveAsync(int? id, TSaveRequest? request, string userName, CancellationToken cancellationToken)
        {
            if (request is null)
                return ServiceResult<TDto>.BadRequest("La solicitud está vacía.");

            TEntity? entity = null;
            if (id is not null)
            {
                entity = await EditQuery.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (entity is null)
                    return ServiceResult<TDto>.NotFound($"{EntityLabel} no existe.");
            }

            Normalize(request);

            var errors = new List<string>();
            await ValidateAsync(request, entity, errors, cancellationToken);
            if (errors.Count > 0)
                return ServiceResult<TDto>.BadRequest("Revise la información capturada.", errors);

            if (entity is null)
            {
                entity = new TEntity { CreatedBy = userName };
                Db.Add(entity);
            }
            else
            {
                entity.UpdatedBy = userName;
            }

            Apply(request, entity);

            try
            {
                await Db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                Db.ChangeTracker.Clear();
                return ServiceResult<TDto>.Conflict($"No se pudo guardar: {ex.InnerException?.Message ?? ex.Message}");
            }

            Db.ChangeTracker.Clear();
            var saved = await ReadQuery.AsNoTracking().FirstAsync(x => x.Id == entity.Id, cancellationToken);

            return id is null
                ? ServiceResult<TDto>.Created(ToDto(saved))
                : ServiceResult<TDto>.Success(ToDto(saved));
        }
    }
}