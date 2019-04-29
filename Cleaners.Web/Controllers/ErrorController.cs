using Cleaners.Web.Localization;
using Cleaners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles error pages occured inside application
    /// </summary>
    [Route("error")]
    public class ErrorController : Controller
    {
        #region Fields

        private readonly IStringLocalizer<ErrorController> _localizer;

        #endregion Fields

        #region Constructor

        public ErrorController(IStringLocalizer<ErrorController> localizer)
        {
            _localizer = localizer;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Handle ajax errors
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [Route("error/{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            return GetView(statusCode);
        }

        private ActionResult GetView(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("NotFound", model: new ErrorModel
                    {
                        StatusCode = statusCode,
                        Title = _localizer[ResourceKeys.NotFoundTitle],
                        Text = _localizer[ResourceKeys.NotFoundText]
                    });

                case 403:
                    return View("Forbidden", model: new ErrorModel
                    {
                        StatusCode = statusCode,
                        Title = _localizer[ResourceKeys.ForbiddenTitle],
                        Text = _localizer[ResourceKeys.ForbiddenText]
                    });

                default:
                    return View("Error", model: new ErrorModel
                    {
                        StatusCode = statusCode,
                        Title = _localizer[ResourceKeys.ErrorTitle],
                        Text = _localizer[ResourceKeys.ErrorText]
                    });
            }
        }

        #endregion Methods
    }
}