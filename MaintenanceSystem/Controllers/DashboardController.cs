using Microsoft.AspNetCore.Mvc;
using MaintenanceSystem.Application.Modules.Dashboard;
using MaintenanceSystem.Common.Results;
using MaintenanceSystem.Web.Extensions;

namespace MaintenanceSystem.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("grid")]
        public IActionResult GetGrid()
        {
            var res = _dashboardService.GetGrid();

            return this.FromServiceResult(res);
        }

    }
}
