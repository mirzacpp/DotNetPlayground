using Microsoft.Extensions.DependencyInjection;

namespace Simplicity.Commons.DependencyInjection
{
	public static partial class TypeExtensions
	{
		/// <summary>
		/// Returns service lifetime based on implemented life time marker for <paramref name="type"/>.
		/// </summary>
		/// <param name="type">Type to retrieve lifetime from</param>
		/// <returns>Service life time</returns>
		public static ServiceLifetime? GetLifeTime(this Type type)
		{
			if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
			{
				return ServiceLifetime.Transient;
			}
			if (typeof(IScopeDependency).GetTypeInfo().IsAssignableFrom(type))
			{
				return ServiceLifetime.Scoped;
			}
			if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
			{
				return ServiceLifetime.Singleton;
			}

			return null;
		}

		public static Type GetRegistrationType(this Type type) =>
		type.IsOpenGenericType() ? type.GetGenericTypeDefinition() : type;

		public static bool IsNotDependencyMarker(this Type type) =>
			type != typeof(ISingletonDependency) &&
			type != typeof(IScopeDependency) &&
			type != typeof(ITransientDependency);
	}
}