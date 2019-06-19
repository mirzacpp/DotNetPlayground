using Cleaners.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public class AdminControllerBase : Controller
    {
        #region Methods

        /// <summary>
        /// Redirects user to previous URL if local, otherwise redirects to home page
        /// </summary>
        /// <returns>Redirect result</returns>
        protected IActionResult RedirectToPreviousUrl()
        {
            var urlReferrer = HttpContext.GetRefererHeader();

            if (urlReferrer == null)
            {
                return Redirect("/");
            }

            var uri = new Uri(urlReferrer);

            if (Url.IsLocalUrl(uri.PathAndQuery))
            {
                return Redirect(urlReferrer);
            }

            return Redirect("/");
        }

        #endregion Methods
    }
}