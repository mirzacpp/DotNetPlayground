using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Provides actions for application settings
    /// </summary>
    [Route("settings")]
    [Authorize(Roles = RoleNames.Admin)]
    public class SettingsController : CorvoControllerBase
    {
        #region Methods

        /// <summary>
        /// Returns page with authentication settings
        /// </summary>
        /// <returns></returns>
        [HttpGet("authentication", Name = SettingsRoutes.AuthenticationSettings)]
        public IActionResult AuthenticationSettings() => View();

        #endregion Methods
    }
}