﻿using Simplicity.AspNetCore.Mvc.Middleware.ClaimsExplore;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Registration methods for <see cref="ClaimsDisplayMiddleware"/> on app startup
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds <see cref="ClaimsDisplayMiddleware"/> to middleware pipeline.
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="pathMatch">Request path to match</param>
    /// <remarks>
    /// Make sure this method is invoked after app.UseAuthentication() in order for middleware to work properly.    
    /// </remarks>
    public static IApplicationBuilder UseClaimsDisplay(this IApplicationBuilder app, string pathMatch)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.Map(pathMatch, conf => conf.UseMiddleware<ClaimsDisplayMiddleware>());

        return app;
    }

    /// <summary>
    /// Adds <see cref="ClaimsDisplayMiddleware"/> to middleware pipeline with on "/claims-display" path.
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <remarks>
    /// Make sure this method is invoked after app.UseAuthentication() in order for middleware to work properly.    
    /// </remarks>
    public static IApplicationBuilder UseClaimsDisplay(this IApplicationBuilder app)
        => UseClaimsDisplay(app, pathMatch: "/claims-display");
}
