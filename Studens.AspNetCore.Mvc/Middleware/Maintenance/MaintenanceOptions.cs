using Microsoft.AspNetCore.Http;

namespace Studens.AspNetCore.Mvc.Middleware.Maintenance
{
    public class MaintenanceOptions
    {
        public MaintenanceOptions()
        {
            IsEnabled = _ => Task.FromResult(false);
            RetryAfterSecondsOffset = 60;
        }

        /// <summary>
        /// Delegate that determines if maintenance is enabled.
        /// Defaults to false.
        /// </summary>
        public Func<HttpContext, Task<bool>> IsEnabled { get; set; }

        /// <summary>
        /// Retry-After header value in seconds
        /// </summary>
        public int RetryAfterSecondsOffset { get; set; }
    }
}