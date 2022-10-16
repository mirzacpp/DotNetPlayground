/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
namespace Microsoft.Extensions.DependencyInjection;

public static partial class CommonServiceCollectionExtensions
{
	/// <summary>
	/// Applies <paramref name="compose"/> on <paramref name="services"/> collection if <paramref name="predicate"/> is true.
	/// </summary>
	/// <param name="services">Services</param>
	/// <param name="predicate">Condition</param>
	/// <param name="compose">Compose delegate</param>
	/// <returns>Current instance of services</returns>
	public static IServiceCollection AddIf(this IServiceCollection services,
	bool predicate,
	Func<IServiceCollection, IServiceCollection> compose)
	{
		if (predicate)
		{
			services = compose(services);
		}

		return services;
	}
}