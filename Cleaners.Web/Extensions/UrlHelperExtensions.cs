using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for UrlHelper
    /// </summary>
    public static class UrlHelperExtensions
    {
        private const string RefererKey = "Referer";

        #region Methods

        public static string PreviousPage(this IUrlHelper urlHelper)
        {
            Guard.Against.Null(urlHelper, nameof(urlHelper));

            // Check if previous url is empty and local and then redirect
            var url = urlHelper.ActionContext.HttpContext.Request.Headers[RefererKey].FirstOrDefault();

            return url;
        }

        #endregion Methods
    }
}