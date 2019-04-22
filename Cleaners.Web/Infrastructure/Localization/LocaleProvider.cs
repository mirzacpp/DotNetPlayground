using Microsoft.Extensions.FileProviders;
using OrchardCore.Localization;
using System;
using System.Collections.Generic;

namespace Cleaners.Web.Infrastructure.Localization
{
    public class LocaleProvider : ILocalizationFileLocationProvider
    {
        public IEnumerable<IFileInfo> GetLocations(string cultureName)
        {
            throw new NotImplementedException();
        }
    }
}