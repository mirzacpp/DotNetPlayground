using Microsoft.AspNetCore.Http;
using Simplicity.AspNetCore.Mvc.Middleware.RegisteredServices;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Configure extenions methods for Registered services
    /// </summary>
    public static class RegisteredServicesExtensions
    {
        /// <summary>
        /// Add dependencies for Registered services middleware
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection AddDisplayRegisteredServices(this IServiceCollection services) =>
            services.AddDisplayRegisteredServices(null);

        /// <summary>
        /// Add dependencies for Registered services middleware
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="path">Path to invoke middleware</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection AddDisplayRegisteredServices(this IServiceCollection services, PathString path)
        {
            services.PostConfigure<RegisteredServicesOptions>(options =>
            {
                if (path != null)
                {
                    options.DefaultPath = path;
                }

                options.Services = new List<ServiceDescriptor>(services);
            });

            return services;
        }
    }
}