using System.Text.Json;
using System.Text.Json.Serialization;

namespace Studens.Data.Extensions
{
    /// <summary>
    /// Json helper to extend <see cref="JsonSerializer"/>.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// JSON serializer options.
        /// For more info on JsonSerializerDefaults.Web see https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-configure-options?pivots=dotnet-6-0#web-defaults-for-jsonserializeroptions
        /// </summary>
        private static readonly JsonSerializerOptions _options = new() // or use new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            //Set to true so we can deserialize properly to C# properties.
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public static TValue? DeserializeFromFile<TValue>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
            }

            var jsonFile = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<TValue>(jsonFile, _options);

            return result;
        }

        public static async Task<TValue?> DeserializeFromFileAsync<TValue>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
            }

            var jsonFile = await File.ReadAllTextAsync(filePath);
            var result = JsonSerializer.Deserialize<TValue>(jsonFile, _options);

            return result;
        }
    }
}