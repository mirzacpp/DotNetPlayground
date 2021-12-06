using StackExchange.Profiling;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for app startup
/// </summary>
public static class CustomServiceCollectionExtensions
{
    /// <summary>
    /// Configures services for MiniProfiler.
    /// For more info see <see cref="https://miniprofiler.com/dotnet/AspDotNetCore"/>
    /// </summary>            
    public static IServiceCollection AddCustomMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.PopupRenderPosition = RenderPosition.BottomLeft;
            options.ColorScheme = ColorScheme.Dark;
            options.EnableDebugMode = true;

            options.IgnoredPaths.Add("/lib");
            options.IgnoredPaths.Add("/css");
            options.IgnoredPaths.Add("/js");
        });

        return services;
    }
}
