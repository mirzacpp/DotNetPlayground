using Microsoft.Extensions.DependencyInjection.Extensions;
using Studens.Extensions.FileProviders;
using Studens.Extensions.FileProviders.FileSystem;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for services registration
/// </summary>
public static class PhysicalFileManagerServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures physical file manager services.
    /// </summary>
    /// <param name="services">Application services</param>
    /// <param name="optionsAction">An action to configure <see cref="PhysicalFileManagerOptions"/></param>
    /// <returns>Application services</returns>
    public static IServiceCollection AddPhysicalFileManager(this IServiceCollection services, Action<PhysicalFileManagerOptions> optionsAction)
    {
        services.TryAddScoped<IFileManager, PhysicalFileManager>();
        services.TryAddScoped<FileProviderErrorDescriber>();
        services.TryAddScoped<FileIOExecutor>();

        services.Configure(optionsAction);

        return services;
    }

    /// <summary>
    /// Adds and configures physical file manager services.
    /// </summary>
    /// <param name="services">Application services</param>
    /// <param name="rootPath">The root directory. This should be an absolute path.</param>
    /// <returns>Application services</returns>
    public static IServiceCollection AddPhysicalFileManager(this IServiceCollection services, string rootPath) =>
        services.AddPhysicalFileManager(options => options.RootPath = rootPath);
}