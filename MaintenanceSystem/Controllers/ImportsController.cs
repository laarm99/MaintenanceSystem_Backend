using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/imports")]
    public class ImportsController : ControllerBase
    {
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;
        private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly IImportService _importService;

        public ImportsController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpGet("definitions")]
        public IActionResult GetDefinitions()
        {
            return Ok(_importService.GetDefinitions());
        }

        [HttpGet("{type}/template")]
        public IActionResult DownloadTemplate(ImportType type)
        {
            var result = _importService.BuildTemplate(type);
            if (!result.IsSuccess) return this.FromServiceResult(result);

            return File(result.Data!, ExcelContentType, $"Plantilla_{type}.xlsx");
        }

        /// <summary>
        /// Validates the uploaded Excel. With commit=true and no row errors, the rows are saved.
        /// </summary>
        [HttpPost("{type}")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<IActionResult> Import(
            ImportType type,
            IFormFile file,
            [FromQuery] bool commit,
            CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { Message = "Seleccione un archivo." });

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { Message = "Solo se permiten archivos .xlsx." });

            await using var stream = file.OpenReadStream();
            var result = await _importService.ImportAsync(
                type, stream, file.FileName, commit, this.GetUserName(), cancellationToken);

            return this.FromServiceResult(result);
        }
    }
}