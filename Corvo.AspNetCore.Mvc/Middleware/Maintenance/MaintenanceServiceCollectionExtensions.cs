using Microsoft.Extensions.DependencyInjection;
using System;

namespace Corvo.AspNetCore.Mvc.Middleware.Maintenance
{
    /// <summary>
    /// Extension methods for easier maintenance configuration
    /// </summary>
    public static class MaintenanceServiceCollectionExtensions
    {
        public static void AddMaintenance(this ServiceCollection services, MaintenanceConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton(configuration);
        }

        public static void AddMaintenance(this ServiceCollection services, Func<bool> isEnabled, byte[] response, int retryAfterSecondsOffset, string contentType)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            AddMaintenance(services, new MaintenanceConfiguration(isEnabled, response)
            {
                ContentType = contentType,
                RetryAfterSecondsOffset = retryAfterSecondsOffset
            });
        }
    }
}