using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for roles
    /// </summary>
    [Authorize(Roles = RoleNames.Admin)]
    [Route("roles")]
    public class RoleController : FealControllerBase
    {
        [HttpGet("", Name = RoleRoutes.Index)]
        public IActionResult Index() => View();
    }
}