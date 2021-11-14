using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Corvo.AspNetCore.Mvc.Middleware.Maintenance
{
    /// <summary>
    /// Middleware that can be used in maintenance cases
    /// when we don't want to allow users to access site
    /// </summary>
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly MaintenanceConfiguration _maintenanceConfiguration;

        public MaintenanceMiddleware(
            RequestDelegate next,
            ILogger logger,
            MaintenanceConfiguration maintenanceConfiguration)
        {
            _next = next;
            _logger = logger;
            _maintenanceConfiguration = maintenanceConfiguration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (_maintenanceConfiguration.IsEnabled)
            {
                // https://yoast.com/http-503-site-maintenance-seo/               
                httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                httpContext.Response.Headers.Add("Retry-After", _maintenanceConfiguration.RetryAfterSecondsOffset.ToString());
                httpContext.Response.ContentType = _maintenanceConfiguration.ContentType;

                await httpContext.Response.WriteAsync(Encoding.UTF8.GetString(_maintenanceConfiguration.Response), Encoding.UTF8);
            }
            
            await _next(httpContext);
        }
    }
}