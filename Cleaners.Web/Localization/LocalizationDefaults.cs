using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cleaners.Web.Localization
{
    public class LocalizationDefaults
    {
        public const string ResourcesPath = "Localization/Resources";
        // Create get method for registered languages

        /// <summary>
        /// Defines list of supported languages
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Language> GetSupportedCultures()
        {
            return new[]
            {
                new Language("en-US", "English", true, false, "en.png"),
                new Language("bs", "Bosnian", false, false, "bs.png")
            };
        }

        /// <summary>
        /// Returns supported languages projected to culture info collection
        /// </summary>
        /// <returns></returns>
        public static IList<CultureInfo> GetSupportedCultureInfos()
        {
            return GetSupportedCultures()
                    .Where(c => !c.IsDisabled)
                    .Select(c => new CultureInfo(c.Name))
                    .ToList();
        }

        public static CultureInfo GetDefaultCultureInfo()
        {
            return GetSupportedCultures()
                    .Where(c => !c.IsDisabled && c.IsDefault)
                    .Select(c => new CultureInfo(c.Name))
                    .FirstOrDefault();
        }
    }
}