using Studens.Commons.DependencyInjection;
using Studens.Commons.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static partial class ServiceCollectionExtensions
{
	#region Methods

	/// <summary>
	/// Registers all classes  within the assemblies that contain the specified <paramref name="assemblyMarkerTypes"/>.
	/// </summary>
	/// <remarks>
	/// Open generics are currently not included.
	/// When class implements multiple interfaces, all of them will be registered except the dependency marker interface. (This can be improved, or use manual registration if default behaviour is not desired.)
	/// In case that class does not implement interface, it will be registered as self.
	/// In case of the generic types, only types with the same arity will be registered ie. Worker<T>/IWorker<T> but not  Worker/IMessage<T>.
	/// TODO: Enable user to expose which interfaces should be registered?
	/// </remarks>
	public static IServiceCollection AutoRegisterMarkedDependencies(this IServiceCollection services, params Type[] assemblyMarkerTypes) =>
	services.AutoRegisterMarkedDependencies(assemblyMarkerTypes.GetAssembliesFromTypes().ToArray());

	/// <summary>
	/// Registers all classes within <paramref name="assembliesToScan"/>.
	/// </summary>
	/// <remarks>
	/// Open generics are currently not included.
	/// When class implements multiple interfaces, all of them will be registered except the dependency marker interface. (This can be improved, or use manual registration if default behaviour is not desired.)
	/// In case that class does not implement interface, it will be registered as self.
	/// In case of the generic types, only types with the same arity will be registered ie. Worker<T>/IWorker<T> but not Worker/IMessage<T>.
	/// TODO: Enable user to expose which interfaces should be registered?
	/// </remarks>
	public static IServiceCollection AutoRegisterMarkedDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
	{
		RegisterDependencies(services, assembliesToScan);
		return services;
	}

	#endregion Methods

	#region Utils

	private static void RegisterDependencies(IServiceCollection services, Assembly[] assembliesToScan)
	{
		Guard.Against.Null(services, nameof(services));

		assembliesToScan = assembliesToScan?.ToArray() ?? Array.Empty<Assembly>();

		if (assembliesToScan.Length > 0)
		{
			// Get all registration candidates
			var implementationTypes = assembliesToScan
				.SelectMany(assembly => AssemblyHelper.GetAllTypes(assembly))
				.Select(type => type.GetRegistrationType())
				.Where(type =>
					type is not null &&
					type.IsClass &&
					!type.IsAbstract &&
					!type.IsAttribute())
				.ToList();

			foreach (var implementationType in implementationTypes)
			{
				var dependencyAttribute = implementationType.GetCustomAttribute<DependencyAttribute>(true);
				var serviceLifetime = dependencyAttribute?.Lifetime ?? implementationType.GetLifeTime();

				if (serviceLifetime is null)
				{
					continue;
				}

				// Get all interfaces for this implementation except registration type
				var serviceTypes = implementationType
					.GetInterfaces()
					.Where(type => type.IsNotDependencyMarker())
					.Where(type => type.HasMatchingGenericArity(implementationType))
					.Select(type => type.GetRegistrationType())
					.ToList();

				// Append self if type does not implement interfaces
				if (!serviceTypes.Any())
				{
					serviceTypes.Add(implementationType);
				}

				foreach (var serviceType in serviceTypes)
				{
					var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, serviceLifetime.Value);

					if (dependencyAttribute?.TryRegister == true)
					{
						services.TryAdd(serviceDescriptor);
					}
					else if (dependencyAttribute?.ReplaceService == true)
					{
						services.Replace(serviceDescriptor);
					}
					else
					{
						services.Add(serviceDescriptor);
					}
				}
			}
		}
	}

	#endregion Utils
}