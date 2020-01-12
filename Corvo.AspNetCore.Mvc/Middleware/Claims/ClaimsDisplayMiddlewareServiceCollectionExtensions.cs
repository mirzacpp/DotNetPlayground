using Microsoft.AspNetCore.Builder;
using System;

namespace Corvo.AspNetCore.Mvc.Middleware.Claims
{
    /// <summary>
    /// Registration methods for <see cref="ClaimsDisplayMiddleware"/> on app startup
    /// </summary>
    public static class ClaimsDisplayMiddlewareServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="ClaimsDisplayMiddleware"/> to middleware pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="pathMatch">Request path to match</param>
        public static void UseClaimsDisplay(this IApplicationBuilder app, string pathMatch)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Map(pathMatch, conf => conf.UseMiddleware<ClaimsDisplayMiddleware>());
        }

        /// <summary>
        /// Adds <see cref="ClaimsDisplayMiddleware"/> to middleware pipeline with on "/claims-display" path.
        /// </summary>
        /// <param name="app">Application builder</param>
        public static void UseClaimsDisplay(this IApplicationBuilder app)
            => UseClaimsDisplay(app, pathMatch: "/claims-display");
    }
}