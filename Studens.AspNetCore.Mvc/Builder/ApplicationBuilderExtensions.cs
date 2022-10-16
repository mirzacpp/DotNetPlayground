namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>
/// </summary>
public static class CommonApplicationBuilderExtensions
{
	/// <summary>
	/// Applies <paramref name="compose"/> on <paramref name="app"/> instance if <paramref name="predicate"/> is true.
	/// </summary>
	/// <param name="app">App builder</param>
	/// <param name="predicate">Condition</param>
	/// <param name="compose">Compose delegate</param>
	/// <returns>Current instance of app builder</returns>
	public static IApplicationBuilder UseIf(this IApplicationBuilder app,
	bool predicate,
	Func<IApplicationBuilder, IApplicationBuilder> compose)
	{
		if (predicate)
		{
			app = compose(app);
		}

		return app;
	}
}