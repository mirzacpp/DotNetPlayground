using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Simplicity.Data.Seed;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class DataSeedServiceCollectionExtensions
{
    /// <summary>
    /// Registers type <typeparamref name="TContributor"/> as an seed contributor.
    /// </summary>
    /// <typeparam name="TContributor">Contributor type</typeparam>
    /// <param name="services">Application service collection.</param>
    /// <returns>Service collection thus enabling method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Service collection is null;
    /// </exception>
    public static IServiceCollection AddDataSeedContributor<TContributor>(this IServiceCollection services)
        where TContributor : IDataSeedContributor
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Note that we register contributor type as self otherwise we won't be able to resolve other contributors.
        services.TryAddTransient(typeof(TContributor));
        // Add type to contributor list.
        services.Configure<DataSeedOptions>(options => options.Contributors.AddIfNotContains(typeof(TContributor)));

        return services;
    }


    /// <summary>
    /// Registers all seed contributors within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/>.
    /// </summary>
    /// <param name="services">Application service collection.</param>
    /// <param name="assemblyMarkerTypes">Assembly marker types to scan.</param>
    /// <returns>Service collection thus enabling method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Service collection is null;
    /// </exception>
    public static IServiceCollection AddDataSeedContributorFromMarkers(this IServiceCollection services, params Type[] assemblyMarkerTypes)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var assembliesToScan = assemblyMarkerTypes?.GetAssembliesFromTypes().ToArray() ?? Array.Empty<Assembly>();

        if (assembliesToScan.Length > 0)
        {
            var targetType = typeof(IDataSeedContributor);
            var contributors = new List<Type>();

            // Get all registrationType implementations avoiding abstract classes.
            var implementations = assembliesToScan
                .SelectMany(a => a.GetTypes())
                .Where(t => targetType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            foreach (var implementation in implementations)
            {
                services.TryAddTransient(implementation);
                contributors.Add(implementation);
            }

            // Add types to contributor list.
            services.Configure<DataSeedOptions>(options => options.Contributors.AddIfNotContains(contributors));
        }

        return services;
    }
}