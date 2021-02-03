using Cleaners.Core.DependencyInjection.Cleaners;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly Assembly[] _emptyAssemblyArray = new Assembly[0];

        /// <summary>
        /// Registers all classes marked with <see cref="ISingletonDependency"/> interface
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddSingletonDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new System.ArgumentNullException(nameof(services));
            }

            assembliesToScan = assembliesToScan?.ToArray() ?? _emptyAssemblyArray;

            if (assembliesToScan?.Length > 0)
            {
                // Get all ISingletonDependency implementations
                //var typesToRegister = assembliesToScan
                //    .SelectMany()
                //    .Where(a => a.GetTypes().Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))).ToList();
            }

            return services;
        }
    }
}