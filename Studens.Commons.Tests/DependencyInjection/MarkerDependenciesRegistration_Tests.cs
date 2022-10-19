using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Studens.Commons.DependencyInjection;

namespace Studens.Commons.Tests.DependencyInjection
{
	public class MarkerDependenciesRegistration_Tests
	{
		[Fact]
		public void ShouldRegisterMarkedDependenciesFromAssembly()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.Count.ShouldBeGreaterThan(0);
		}

		[Fact]
		public void ShouldRegisterMarkedDependenciesAsSelf()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.ShouldContainTransientDependency(typeof(TransientAsSelfClass));
			services.ShouldContainScopedDependency(typeof(ScopedAsSelfClass));
			services.ShouldContainSingletonDependency(typeof(SingletonAsSelfClass));
		}

		[Fact]
		public void ShouldRegisterMarkedDependenciesAsImplementation()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.ShouldContainTransientDependency(typeof(IContractor), typeof(TransientImplClass));
			services.ShouldContainScopedDependency(typeof(IContractor), typeof(ScopedImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractor), typeof(SingletonImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractor2), typeof(SingletonImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractorGen<,>), typeof(SingletonOpenGenImplClass<,>));
		}

		[Fact]
		public void ShouldReplaceExistingDependencyIfSpecified()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.ShouldContainScopedDependency(typeof(IContractorReplacement), typeof(ReplacementScopedImplClass));
			services.ShouldNotContainDependency(typeof(ScopedImplToReplaceClass));
		}

		[Fact]
		public void ShouldNotRegisterMultipleImplementationDependenciesIfSpecified()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.ShouldContainScopedDependency(typeof(IContractor), typeof(ScopedImplClass));
			services.ShouldNotContainDependency(typeof(TryRegisterScopedImplClass));
		}

		[Fact]
		public void ShouldNotRegisterDependenciesWithDifferentGenericArities()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistration_Tests));

			//Assert
			services.ShouldContainScopedDependency(typeof(IContractor), typeof(ScopedNotificationHandler));
			services.ShouldNotContainDependency(typeof(INotification<string>), typeof(ScopedNotificationHandler));
		}
	}

	#region Types

	public class TransientAsSelfClass : ITransientDependency
	{ }

	public class ScopedAsSelfClass : IScopeDependency
	{ }

	public class SingletonAsSelfClass : ISingletonDependency
	{ }

	public class TransientImplClass : IContractor, ITransientDependency
	{ }

	public class ScopedImplClass : IContractor, IScopeDependency
	{ }

	public class SingletonImplClass : IContractor, IContractor2, ISingletonDependency
	{ }

	public class ScopedImplToReplaceClass : IContractorReplacement, IScopeDependency
	{ }

	[Dependency(ReplaceService = true)]
	public class ReplacementScopedImplClass : IContractorReplacement, IScopeDependency
	{ }

	[Dependency(TryRegister = true)]
	public class TryRegisterScopedImplClass : IContractor, IScopeDependency
	{ }

	public class SingletonOpenGenImplClass<T1, T2> : IContractorGen<T1, T2>, ISingletonDependency
	{ }

	public class ScopedNotificationHandler : IContractor, INotification<string>, IScopeDependency
	{ }

	public interface IContractor
	{ }

	public interface IContractorReplacement
	{ }

	public interface IContractor2
	{ }

	public interface IContractorGen<T1, T2>
	{ }

	public interface INotification<T>
	{ }

	#endregion Types
}