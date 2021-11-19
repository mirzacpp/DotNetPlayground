﻿namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>
/// </summary>
public static class CommonApplicationBuilderExtensions
{
    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        bool predicate,
        Func<IApplicationBuilder> compose) => predicate ? compose() : app;

    //public static IApplicationBuilder UseIf(this IApplicationBuilder app,
    //    bool predicate,
    //    Action<IApplicationBuilder> compose)
    //{
    //    if (predicate)
    //    {
    //        compose(app);
    //    }

    //    return app;
    //}
}
