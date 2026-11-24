using Microsoft.AspNetCore.Mvc;
using MaintenanceSystem.Common.Results;
using System.Net;

namespace MaintenanceSystem.Web.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Domain user name without the domain, upper case (same format as IDM_ACCOUNT.DOMAIN_USER_NAME).
        /// </summary>
        public static string GetUserName(this ControllerBase controller)
        {
            return controller.User.Identity?.Name?
                .Split('\\')
                .Last()
                .ToUpperInvariant() ?? string.Empty;
        }

        public static IActionResult FromServiceResult<T>(
       this ControllerBase controller,
       ServiceResult<T> result)
        {
            return result.StatusCode switch
            {
                HttpStatusCode.OK =>
                    controller.Ok(result.Data),

                HttpStatusCode.Created =>
                    controller.Created(string.Empty, result.Data),

                HttpStatusCode.BadRequest =>
                    controller.BadRequest(new
                    {
                        result.Message,
                        result.Errors
                    }),

                HttpStatusCode.NotFound =>
                    controller.NotFound(new
                    {
                        result.Message
                    }),

                HttpStatusCode.Conflict =>
                    controller.Conflict(new
                    {
                        result.Message
                    }),

                HttpStatusCode.InternalServerError =>
                    controller.StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        result.Message
                    }),

                _ =>
                    controller.StatusCode((int)result.StatusCode,
                    new
                    {
                        result.Message
                    })
            };
        }
    }
}
