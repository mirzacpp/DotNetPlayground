using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Corvo.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/>
    /// </summary>
    public static class HttpContextExtensions
    {
        #region Constants

        public const string RefererHeader = "Referer";
        public const string AjaxRequestHeader = "X-Requested-With";
        public const string AjaxRequestHeaderValue = "XMLHttpRequest";

        #endregion Constants

        #region Methods

        public static string GetRefererHeader(this HttpContext httpContext)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            return httpContext.Request.Headers[RefererHeader].FirstOrDefault();
        }

        /// <summary>
        /// Determines if request is send as ajax request
        /// </summary>
        /// <param name="httpRequest">Current request</param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            if (httpRequest.Headers.TryGetValue(AjaxRequestHeader, out StringValues value))
            {
                return value == AjaxRequestHeaderValue;
            }

            return false;
        }

        #endregion Methods
    }
}