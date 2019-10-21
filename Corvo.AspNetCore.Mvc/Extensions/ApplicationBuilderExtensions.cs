using Microsoft.AspNetCore.Builder;
using System;

namespace Corvo.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers middleware only when given condition is statistfied, and returns instance
        /// Used for cleaner middleware registration
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IApplicationBuilder When(this IApplicationBuilder appBuilder, bool condition, Func<IApplicationBuilder> compose)
        {
            return condition ? compose() : appBuilder;
        }

        /// <summary>
        /// Executes given operation if condition is statisfied
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="condition"></param>
        /// <param name="compose"></param>
        public static void When(this IApplicationBuilder appBuilder, bool condition, Action compose)
        {
            if (condition)
            {
                compose();
            }
        }
    }
}