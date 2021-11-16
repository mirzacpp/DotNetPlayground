using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Studens.Commons.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static partial class ServiceCollectionExtensions
{
    #region Fields

    private static readonly Assembly[] _emptyAssemblyArray = Array.Empty<Assembly>();

    /// <summary>
    /// Delegate for interface registration strategy
    /// </summary>
    private static readonly Func<Type, Type, bool> _interfaceRegistrationStrategy = (targetType, registrationType) =>
                                   targetType.IsClass && registrationType.IsAssignableFrom(targetType);

    /// <summary>
    /// Delegate for attribute registration strategy
    /// </summary>
    private static readonly Func<Type, Type, bool> _attributeRegistrationStrategy = (targetType, attributeType) =>
                                   targetType.IsClass && targetType.GetCustomAttribute(attributeType, false) != null;

    #endregion Fields

    #region Methods

    #region Interface(marker) registration

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> marked with <see cref="ISingletonDependency"/> interface.
    /// </summary>
    public static IServiceCollection AddSingletonDependenciesFromMarkers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services,
            typeof(ISingletonDependency),
            ServiceLifetime.Singleton,
            assembliesToScan,
            _interfaceRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> marked with <see cref="ISingletonDependency"/> interface.
    /// </summary>    
    public static IServiceCollection AddSingletonDependenciesFromMarkers(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddSingletonDependenciesFromMarkers(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> marked with <see cref="IScopeDependency"/> interface.
    /// </summary>
    public static IServiceCollection AddScopedDependenciesFromMarkers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services, 
            typeof(IScopeDependency),
            ServiceLifetime.Scoped,
            assembliesToScan,
            _interfaceRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> marked with <see cref="IScopeDependency"/> interface.
    /// </summary>    
    public static IServiceCollection AddScopedDependenciesFromMarkers(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddScopedDependenciesFromMarkers(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> marked with <see cref="ITransientDependency"/> interface.
    /// </summary>
    public static IServiceCollection AddTransientDependenciesFromMarkers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services, 
            typeof(ITransientDependency),
            ServiceLifetime.Transient, 
            assembliesToScan,
            _interfaceRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> marked with <see cref="ITransientDependency"/> interface.
    /// </summary>  
    public static IServiceCollection AddTransientDependenciesFromMarkers(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddTransientDependenciesFromMarkers(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    #endregion Interface(marker) registration

    #region Attribute registrations

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> decorated with <see cref="SingletonDependencyAttribute"/> attribute.
    /// </summary>
    public static IServiceCollection AddSingletonDependenciesFromAttributes(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services,
            typeof(SingletonDependencyAttribute),
            ServiceLifetime.Singleton,
            assembliesToScan,
            _attributeRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> decorated with <see cref="SingletonDependencyAttribute"/> attribute.
    /// </summary>  
    public static IServiceCollection AddSingletonDependenciesAttributes(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddSingletonDependenciesFromAttributes(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> decorated with <see cref="ScopeDependencyAttribute"/> attribute.
    /// </summary>
    public static IServiceCollection AddScopedDependenciesFromAttributes(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services,
            typeof(ScopeDependencyAttribute),
            ServiceLifetime.Scoped,
            assembliesToScan,
            _attributeRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> decorated with <see cref="ScopeDependencyAttribute"/> attribute.
    /// </summary>  
    public static IServiceCollection AddScopedDependenciesFromAttributes(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddScopedDependenciesFromAttributes(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes within <paramref name="assembliesToScan"/> decorated with <see cref="TransientDependencyAttribute"/> attribute.
    /// </summary>
    public static IServiceCollection AddTransientDependenciesFromAttributes(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        RegisterDependencies(services,
            typeof(TransientDependencyAttribute),
            ServiceLifetime.Transient,
            assembliesToScan,
            _attributeRegistrationStrategy);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> decorated with <see cref="TransientDependencyAttribute"/> attribute.
    /// </summary>  
    public static IServiceCollection AddTransientDependenciesFromAttributes(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddTransientDependenciesFromAttributes(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    #endregion Attribute registrations

    /// <summary>
    /// Registers classes for all lifetime types marked with one of the markers defined within <paramref name="assembliesToScan"/>
    /// </summary>    
    public static IServiceCollection AddAllDependenciesFromMarkers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        AddSingletonDependenciesFromMarkers(services, assembliesToScan);
        AddScopedDependenciesFromMarkers(services, assembliesToScan);
        AddTransientDependenciesFromMarkers(services, assembliesToScan);

        return services; 
    }

    /// <summary>
    /// Registers classes for all lifetime types marked with one of the markers from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddAllDependenciesFromMarkers(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddAllDependenciesFromMarkers(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> decorated with any of the dependency attributes.
    /// </summary> 
    public static IServiceCollection AddAllDependenciesFromAttributes(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        AddSingletonDependenciesFromAttributes(services, assembliesToScan);
        AddScopedDependenciesFromAttributes(services, assembliesToScan);
        AddTransientDependenciesFromAttributes(services, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers all classes within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/> decorated with any of the dependency attributes.
    /// </summary>
    public static IServiceCollection AddAllDependenciesFromAttributes(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddAllDependenciesFromAttributes(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    /// <summary>
    /// Registers classes for all lifetime types including both, markers and attribute strategies defined within <paramref name="assembliesToScan"/>
    /// </summary>
    public static IServiceCollection AddAllDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        AddSingletonDependenciesFromMarkers(services, assembliesToScan);
        AddScopedDependenciesFromMarkers(services, assembliesToScan);
        AddTransientDependenciesFromMarkers(services, assembliesToScan);

        AddSingletonDependenciesFromAttributes(services, assembliesToScan);
        AddScopedDependenciesFromAttributes(services, assembliesToScan);
        AddTransientDependenciesFromAttributes(services, assembliesToScan);

        return services;
    }

    /// <summary>
    /// Registers classes for all lifetime types including both, markers and attribute strategies from the assemblies that contain the specified types
    /// </summary>
    public static IServiceCollection AddAllDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
        AddAllDependencies(services, assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

    #endregion Methods

    #region Utils

    /// <summary>
    /// Register dependencies of <paramref name="registrationType"/> type with <paramref name="serviceLifetime"/> lifetime in <paramref name="assembliesToScan"/>
    /// </summary>
    private static void RegisterDependencies(IServiceCollection services,
        Type registrationType,
        ServiceLifetime serviceLifetime,
        Assembly[] assembliesToScan,
        Func<Type, Type, bool> registrationStrategy)
    {
        Guard.Against.Null(services, nameof(services)); 

        assembliesToScan = assembliesToScan?.ToArray() ?? _emptyAssemblyArray;

        if (assembliesToScan.Length > 0)
        {
            // Get all registrationType implementations
            var implementations = assembliesToScan
                .SelectMany(a => a.GetTypes())
                .Where(t => registrationStrategy(t, registrationType))                
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

    #endregion Utils
}