using Microsoft.Extensions.DependencyInjection;

namespace Studens.Commons.DependencyInjection
{
	/// <summary>
	/// Allows dependency to be decorated wih additional information.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class DependencyAttribute : Attribute
	{
		/// <summary>
		/// Indicates that dependency should be only registered if not already registered.
		/// </summary>
		public bool TryRegister { get; set; }

		/// <summary>
		/// Indicates that previously registered dependency should be replaced with current one.
		/// </summary>
		public bool ReplaceService { get; set; }

		/// <summary>
		/// Specifies lifetime of the dependency.
		/// </summary>
		public ServiceLifetime? Lifetime { get; set; }

		public DependencyAttribute()
		{
		}

		public DependencyAttribute(ServiceLifetime lifetime)
		{
			Lifetime = lifetime;
		}
	}
}