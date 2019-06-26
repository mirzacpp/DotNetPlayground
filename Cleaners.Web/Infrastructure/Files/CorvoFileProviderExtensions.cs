using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Extension methods for file provider configuration at startup
    /// </summary>
    public static class CorvoFileProviderExtensions
    {
        public static void AddCorvoFileProvider(this IServiceCollection services, Action<CorvoFileProviderOptions> configuration = null)
        {
            services.AddSingleton<ICorvoFileProvider>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<CorvoFileProviderOptions>>().Value;

                // Invoke additional configuration from delegate if any
                configuration?.Invoke(options);

                // Make sure root path is configured
                if (string.IsNullOrEmpty(options.BasePath))
                {
                    throw new InvalidOperationException($"Value for {nameof(options.BasePath)} cannot be null or empty.");
                }

                // If directory with base path doesn't exists, application will throw DirectoryNotFoundException excepion
                Directory.CreateDirectory(options.BasePath);

                return new CorvoFileProvider(options);
            });
        }
    }
}