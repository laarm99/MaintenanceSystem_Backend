using MaintenanceSystem.Application.Modules.Common;
using MaintenanceSystem.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Web.Controllers
{
    /// <summary>
    /// GET / GET {id} / POST / PUT {id} / DELETE {id} over an <see cref="ICrudService{TDto, TSaveRequest}"/>.
    /// Derived controllers only declare the route.
    /// </summary>
    [Authorize]
    [ApiController]
    public abstract class CrudControllerBase<TDto, TSaveRequest> : ControllerBase
    {
        private readonly ICrudService<TDto, TSaveRequest> _service;

        protected CrudControllerBase(ICrudService<TDto, TSaveRequest> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
            Ok(await _service.GetAllAsync(cancellationToken));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) =>
            this.FromServiceResult(await _service.GetByIdAsync(id, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TSaveRequest request, CancellationToken cancellationToken) =>
            this.FromServiceResult(await _service.CreateAsync(request, this.GetUserName(), cancellationToken));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TSaveRequest request, CancellationToken cancellationToken) =>
            this.FromServiceResult(await _service.UpdateAsync(id, request, this.GetUserName(), cancellationToken));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
            this.FromServiceResult(await _service.DeleteAsync(id, cancellationToken));
    }
}