using Ardalis.GuardClauses;
using Cleaners.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cleaners.DependencyInjection.Extensions
{
    /// <summary>
    /// This class extends base <see cref="IServiceCollection"/> with Structor library
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedsFromAssembly(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));            

            return services;
        }
    }
}