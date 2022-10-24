using Microsoft.Extensions.DependencyInjection;

namespace Simplicity.AspNetCore.Mvc.Razor;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="RazorViewToStringRenderer"/> for current service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddRazorViewToStringRenderer(this IServiceCollection services)
    {
        services.AddScoped<RazorViewToStringRenderer>();

        return services;
    }
}