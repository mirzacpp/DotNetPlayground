using Microsoft.AspNetCore.Identity;
using StackExchange.Profiling;
using Studens.AspNetCore.Identity;

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
	public static IServiceCollection AddMiniProfilerConfigured(this IServiceCollection services)
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

	public static IServiceCollection AddIdentityConfigured(this IServiceCollection services)
	{
		services.AddIdentity<IdentityUser, IdentityRole>()
			 .AddUserStore<IIdentityUserStore<IdentityUser>>()
			 .AddUserManager<IdentityUserManager<IdentityUser>>();

		return services;
	}
}