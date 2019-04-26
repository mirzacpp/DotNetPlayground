using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for HttpContext
    /// </summary>
    public static class HttpContextExtensions
    {
        #region Methods

        public static string GetRefererHeader(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["Referer"].FirstOrDefault();
        }

        #endregion Methods
    }
}