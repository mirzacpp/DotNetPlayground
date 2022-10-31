using Microsoft.Extensions.DependencyInjection;
using Simplicity.Commons.DependencyInjection;

namespace Simplicity.Commons.Tests.DependencyInjection
{
	public class MarkerDependenciesRegistrationTests
	{
		[Fact]
		public void Register_marked_dependencies_from_assembly()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

			//Assert
			services.Count.ShouldBeGreaterThan(0);
		}

		[Fact]
		public void Register_marked_dependencies_as_self()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

			//Assert
			services.ShouldContainTransientDependency(typeof(TransientAsSelfClass));
			services.ShouldContainScopedDependency(typeof(ScopedAsSelfClass));
			services.ShouldContainSingletonDependency(typeof(SingletonAsSelfClass));
		}

		[Fact]
		public void Register_marked_dependencies_as_implementation()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

			//Assert
			services.ShouldContainTransientDependency(typeof(IContractor), typeof(TransientImplClass));
			services.ShouldContainScopedDependency(typeof(IContractor), typeof(ScopedImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractor), typeof(SingletonImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractor2), typeof(SingletonImplClass));
			services.ShouldContainSingletonDependency(typeof(IContractorGen<,>), typeof(SingletonOpenGenImplClass<,>));
		}

		[Fact]
		public void Replace_existing_dependency_if_specified()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

			//Assert
			services.ShouldContainScopedDependency(typeof(IContractorReplacement), typeof(ReplacementScopedImplClass));
			services.ShouldNotContainDependency(typeof(ScopedImplToReplaceClass));
		}

		[Fact]
		public void Do_not_register_multiple_implementation_dependencies_if_specified()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

			//Assert
			services.ShouldContainScopedDependency(typeof(IContractor), typeof(ScopedImplClass));
			services.ShouldNotContainDependency(typeof(TryRegisterScopedImplClass));
		}

		[Fact]
		public void Do_not_register_dependencies_with_different_generic_arities()
		{
			//Arrange
			var services = new ServiceCollection();

			//Act
			services.AutoRegisterMarkedDependencies(typeof(MarkerDependenciesRegistrationTests));

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