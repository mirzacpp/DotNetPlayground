using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Studens.Extensions.FileProviders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>
/// </summary>
public static class PhysicalFileManagerApplicationBuilderExtensions
{
    /// <summary>
    /// Enables browser to serve files managed by <see cref="PhysicalFileManager"/>.
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="requestPath">The relative request path that maps to static resources.</param>
    /// <returns>Application builder</returns>
    public static IApplicationBuilder UsePhysicalFileManagerBrowser(this IApplicationBuilder app, PathString requestPath)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<PhysicalFileManagerOptions>>();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(options.Value.RootPath),
            RequestPath = requestPath
        });

        return app;
    }
}