using Corvo.AspNetCore.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class CorvoControllerBase : Controller
    {
        #region Methods

        /// <summary>
        /// Redirects user to previous URL if local, otherwise redirects to home page.
        /// </summary>
        /// <returns>Redirect result</returns>
        protected IActionResult RedirectToPreviousUrl()
            => Redirect(Url.GetRefererOrFallback());

        #endregion Methods
    }
}