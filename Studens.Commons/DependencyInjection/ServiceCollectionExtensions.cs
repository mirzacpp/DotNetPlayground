using Microsoft.Extensions.DependencyInjection.Extensions;
using Studens.Commons.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    #region Fields

    private static readonly Assembly[] _emptyAssemblyArray = Array.Empty<Assembly>();

    #endregion Fields

    /// <summary>
    /// Registers all classes marked with <see cref="ISingletonDependency"/> interface defined within <paramref name="assembliesToScan"/>
    /// </summary>
    public static IServiceCollection AddSingletonDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services, typeof(ISingletonDependency), ServiceLifetime.Singleton, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers all classes marked with <see cref="ISingletonDependency"/> interface from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddSingletonDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddSingletonDependencies(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes marked with <see cref="IScopeDependency"/> interface defined within <paramref name="assembliesToScan"/>
    /// </summary>
    public static IServiceCollection AddScopedDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services, typeof(IScopeDependency), ServiceLifetime.Scoped, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers all classes marked with <see cref="IScopeDependency"/> interface from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddScopedDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddScopedDependencies(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes marked with <see cref="ITransientDependency"/> interface defined within <paramref name="assembliesToScan"/>
    /// </summary>
    public static IServiceCollection AddTransientDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services, typeof(ITransientDependency), ServiceLifetime.Transient, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers all classes marked with <see cref="ITransientDependency"/> interface from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddTransientDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddTransientDependencies(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers classes for all lifetime types defined within <paramref name="assembliesToScan"/>
    /// </summary>
    public static IServiceCollection AddAllDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        AddSingletonDependencies(services, assembliesToScan);
        AddScopedDependencies(services, assembliesToScan);
        AddTransientDependencies(services, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers classes for all lifetime types from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddAllDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddAllDependencies(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Register dependencies of <paramref name="registrationType"/> type with <paramref name="serviceLifetime"/> lifetime in <paramref name="assembliesToScan"/>
    /// </summary>
    /// <remarks>
    /// TODO: Registration is not working when there is a open generic constraint ?
    /// This is ok, interfaces without constraints should go at the begining
    /// </remarks>
    private static void RegisterDependencies(IServiceCollection services,
        Type registrationType,
        ServiceLifetime serviceLifetime,
        Assembly[] assembliesToScan)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        assembliesToScan = assembliesToScan?.ToArray() ?? _emptyAssemblyArray;

        if (assembliesToScan.Length > 0)
        {
            // Get all registrationType implementations           
            var implementations = assembliesToScan
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && registrationType.IsAssignableFrom(t))
                .ToList();

            foreach (var implementation in implementations)
            {
                // Get all interfaces for this implementation except registration type
                var contracts = implementation.GetInterfaces()
                    .Where(t => t != registrationType)
                    .ToList();

                // Register as constructed type or as type definition/template
                var typeToRegister = implementation.GetGenericTypeDefinitionOrDefault();

                if (contracts.Any())
                {
                    foreach (var contract in contracts)
                    {
                        services.TryAdd(new ServiceDescriptor(
                            contract.GetGenericTypeDefinitionOrDefault(), typeToRegister, serviceLifetime));
                    }
                }
                // Otherwise, register type as itself
                else
                {
                    services.TryAdd(new ServiceDescriptor(typeToRegister, typeToRegister, serviceLifetime));
                }
            }
        }
    }
}
