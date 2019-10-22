using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Corvo.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IUrlHelper"/>
    /// </summary>
    public static class UrlHelperExtensions
    {
        public static string GetRefererOrFallback(this IUrlHelper urlHelper)
            => GetRefererOrFallback(urlHelper, null, null);

        public static string GetRefererOrFallback(this IUrlHelper urlHelper, string route)
            => GetRefererOrFallback(urlHelper, route, null);

        public static string GetRefererOrFallback(this IUrlHelper urlHelper, string route, object values)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            var referer = urlHelper.ActionContext.HttpContext.Request.Headers["Referer"].FirstOrDefault();

            if (!string.IsNullOrEmpty(referer))
            {
                var url = new Uri(referer);

                if (urlHelper.IsLocalUrl(url.AbsolutePath))
                {
                    return referer;
                }
            }

            return string.IsNullOrEmpty(route) ?
                    "~/" :
                    urlHelper.RouteUrl(route, values);
        }
    }
}