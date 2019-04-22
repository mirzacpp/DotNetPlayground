using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Cleaners.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureLocalization(this IApplicationBuilder builder)
        {
            builder.UseRequestLocalization();
        }
    }
}