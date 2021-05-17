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
        public static IServiceCollection AddSingletonDependencies(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            assembliesToScan = assembliesToScan?.ToArray() ?? _emptyAssemblyArray;

            if (assembliesToScan?.Length > 0)
            {
                var registrationType = typeof(ISingletonDependency);

                // Get all ISingletonDependency implementations
                var typesToRegister = assembliesToScan
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && registrationType.IsAssignableFrom(t))
                    .ToList();

                foreach (var type in typesToRegister)
                {
                    Console.WriteLine($"Class: {type}");

                    //var trusi = type.IsGenericTypeDefinition;

                    //Type service = type.IsGenericType
                    //&& type.IsGenericTypeDefinition
                    //&& type.ContainsGenericParameters
                    //? type.GetGenericTypeDefinition()
                    //: type;

                    // Get all interface types for this implementation except registration type
                    var implementations = type.GetInterfaces()
                        .Where(t => t != registrationType)
                        .ToList();

                    // Handle case for unbound generics
                    if (type.IsGenericTypeDefinition)
                    {
                        foreach (var implementation in implementations)
                        {
                            services.Add(new ServiceDescriptor(
                                implementation.GetGenericTypeDefinition(), type.GetGenericTypeDefinition(), ServiceLifetime.Singleton));
                        }
                    }
                    else
                    {
                        foreach (var implementation in implementations)
                        {
                            if (type.IsGenericTypeDefinition)
                            {
                                services.Add(new ServiceDescriptor(
                                    implementation.GetGenericTypeDefinition(),
                                    type.GetGenericTypeDefinition(),
                                    ServiceLifetime.Singleton));
                            }
                            else
                            {
                                services.Add(new ServiceDescriptor(implementation, type, ServiceLifetime.Singleton));
                            }
                        }
                    }

                    //if (!services.IsAdded(type))
                    //{
                    //    services.Add(new ServiceDescriptor(type, ))
                    //}
                }
            }

            return services;
        }
    }
}