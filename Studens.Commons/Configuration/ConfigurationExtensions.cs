namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for Microsoft configuration 
    /// Credits to <see cref="https://josef.codes/jos-configuration-convenient-methods-for-configuration-in-dotnet-core/"/>
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Extracts the required value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <exception cref="InvalidOperationException">
        /// Section with the given key was not found.
        /// </exception>
        /// <returns>The converted value.</returns>
        public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        {
            var value = configuration.GetValue<T>(key) ??
                throw new InvalidOperationException($"'{key}' had no value, make sure it has been added to the Configuration.");

            return value;
        }


        /// <summary>
        /// Extracts the required values with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the values to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's values to convert.</param>
        /// <exception cref="InvalidOperationException">
        /// Section with the given key was not found.
        /// </exception>
        /// <returns>The converted values.</returns>
        public static IEnumerable<T> GetRequiredValues<T>(this IConfiguration configuration, string key)
        {
            var configurationSection = configuration.GetRequiredSection(key);
            var values = new List<T>();
            configurationSection.Bind(values);

            return values;
        }


        /// <summary>
        /// Attempts to bind the given object instance to configuration values by matching
        /// property names against configuration keys recursively.
        /// </summary>
        /// <typeparam name="T">The type to convert the values to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's values to convert.</param>
        /// <exception cref="InvalidOperationException">
        /// Section with the given key was not found.
        /// </exception>
        /// <returns>The converted values.</returns>
        public static T GetRequiredOptions<T>(this IConfiguration configuration, string key) where T : new()
        {
            var configurationSection = configuration.GetRequiredSection(key);
            var options = new T();
            configurationSection.Bind(options);

            return options;
        }
    }
}
