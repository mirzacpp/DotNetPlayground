using Corvo.AspNetCore.Mvc.ActionResults;
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
        [NonAction]
        protected IActionResult RedirectToPreviousUrl()
            => Redirect(Url.GetRefererOrFallback());

        [NonAction]
        protected IActionResult AjaxRedirectToActionResult(string actionName)
            => new AjaxRedirectResult(Url.Action(actionName, null, null));

        [NonAction]
        protected IActionResult AjaxRedirectToActionResult(string actionName, string controllerName)
            => new AjaxRedirectResult(Url.Action(actionName, controllerName, null));

        [NonAction]
        protected IActionResult AjaxRedirectToActionResult(string actionName, string controllerName, object routeValues)
            => new AjaxRedirectResult(Url.Action(actionName, controllerName, routeValues));

        [NonAction]
        protected IActionResult AjaxRedirectToRouteResult(string routeName)
            => AjaxRedirectToRouteResult(routeName, null);

        [NonAction]
        protected IActionResult AjaxRedirectToRouteResult(string routeName, object routeValues)
            => new AjaxRedirectResult(Url.RouteUrl(routeName, routeValues));

        #endregion Methods
    }
}