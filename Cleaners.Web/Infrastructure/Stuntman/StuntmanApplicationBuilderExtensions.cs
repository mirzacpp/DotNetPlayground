using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RimDev.Stuntman.Core;
using System;

namespace Cleaners.Web.Infrastructure.Stuntman
{
    /// <summary>
    /// Extension methods for Stuntman Middleware
    /// </summary>
    public static class StuntmanApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseStuntman(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder is null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            var options = applicationBuilder.ApplicationServices.GetService<StuntmanOptions>();

            if (options is null)
            {
                throw new NullReferenceException("Stuntman options cannot be null.");
            }

            applicationBuilder.UseStuntman(options);

            return applicationBuilder;
        }
    }
}