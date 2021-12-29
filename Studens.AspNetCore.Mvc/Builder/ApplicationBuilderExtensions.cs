namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>
/// </summary>
public static class CommonApplicationBuilderExtensions
{
    /// <summary>
    /// Applies <paramref name="compose"/> if <paramref name="predicate"/> is true.
    /// </summary>
    /// <param name="app">App builder</param>
    /// <param name="predicate">Condition</param>
    /// <param name="compose">Compose delegate</param>
    /// <returns>App builder</returns>
    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        bool predicate,
        Func<IApplicationBuilder> compose) => predicate ? compose() : app;
}