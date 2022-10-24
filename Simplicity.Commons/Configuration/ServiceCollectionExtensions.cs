namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// Credits to <see cref="https://josef.codes/jos-configuration-convenient-methods-for-configuration-in-dotnet-core/"/>
    /// </summary>
    public static partial class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <typeparamref name="T"/> options as an POCO object.
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Section with the given key was not found.
        /// </exception>
        public static IServiceCollection AddPocoOptions<T>(this IServiceCollection services,
            string key,
            IConfiguration configuration) where T : class, new()
        {
            var options = configuration.GetRequiredOptions<T>(key);
            services.AddSingleton(options);

            return services;
        }

        /// <summary>
        /// Registers <typeparamref name="T"/> options as an POCO object.
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="options">Binded options to return</param>
        /// <exception cref="InvalidOperationException">
        /// Section with the given key was not found.
        /// </exception>
        public static IServiceCollection AddPocoOptions<T>(this IServiceCollection services,
            string key,
            IConfiguration configuration,
            out T options) where T : class, new()
        {
            options = configuration.GetRequiredOptions<T>(key);
            services.AddSingleton(options);

            return services;
        }
    }
}