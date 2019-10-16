using Microsoft.AspNetCore.Builder;

namespace Corvo.AspNetCore.Mvc.Middleware.Maintenance
{
    /// <summary>
    /// Extensions methods for easier <see cref="MaintenanceMiddleware"/> registration
    /// </summary>
    public static class MaintenanceApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers <see cref="MaintenanceMiddleware"/> middleware
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMaintenance(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}