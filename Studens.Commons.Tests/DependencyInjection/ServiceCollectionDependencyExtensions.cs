using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Studens.Commons.Tests.DependencyInjection
{
	internal static class ServiceCollectionDependencyExtensions
	{
		public static void ShouldContainTransientDependency(this IServiceCollection services,
		Type serviceType,
		Type? implementationType = null)
		{
			var serviceDescriptor = GetServiceDescriptor(services, serviceType, implementationType);

			AssertBase(serviceDescriptor, serviceType, implementationType);
			serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
		}

		public static void ShouldContainScopedDependency(this IServiceCollection services,
		Type serviceType,
		Type? implementationType = null)
		{
			var serviceDescriptor = GetServiceDescriptor(services, serviceType, implementationType);

			AssertBase(serviceDescriptor, serviceType, implementationType);
			serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
		}

		public static void ShouldContainSingletonDependency(this IServiceCollection services,
		Type serviceType,
		Type? implementationType = null)
		{
			var serviceDescriptor = GetServiceDescriptor(services, serviceType, implementationType);

			AssertBase(serviceDescriptor, serviceType, implementationType);
			serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
		}

		public static void ShouldNotContainDependency(this IServiceCollection services, Type serviceType, Type? implementationType = null)
		{
			var serviceDescriptor = GetServiceDescriptor(services, serviceType, implementationType);

			serviceDescriptor.ShouldBeNull();
		}

		private static ServiceDescriptor GetServiceDescriptor(IServiceCollection services, Type serviceType,
		Type? implementationType = null)
		{
			return services
				.FirstOrDefault(sd => sd.ServiceType == serviceType
				&& (implementationType is null || sd.ImplementationType == implementationType));
		}

		private static void AssertBase(ServiceDescriptor serviceDescriptor, Type serviceType, Type? implementationType = null)
		{
			serviceDescriptor.ShouldNotBeNull();
			serviceDescriptor.ImplementationType.ShouldBe(implementationType ?? serviceType);
			serviceDescriptor.ImplementationFactory.ShouldBeNull();
			serviceDescriptor.ImplementationInstance.ShouldBeNull();
		}
	}
}