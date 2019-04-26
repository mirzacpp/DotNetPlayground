using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for UrlHelper
    /// </summary>
    public static class UrlHelperExtensions
    {
        #region Methods

        public static string PreviousPage(this IUrlHelper urlHelper)
        {
            // Check if previous url is empty and local and then redirect
            var url = urlHelper.ActionContext.HttpContext.Request.Headers["Referer"].FirstOrDefault();

            return url;
        }

        #endregion Methods
    }
}