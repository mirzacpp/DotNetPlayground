using Microsoft.AspNetCore.Builder;

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