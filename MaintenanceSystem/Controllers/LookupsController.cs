using MaintenanceSystem.Application.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/lookups")]
    public class LookupsController(ILookupService lookupService) : ControllerBase
    {
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken) =>
            Ok(await lookupService.GetActiveAccountsAsync(cancellationToken));
    }
}