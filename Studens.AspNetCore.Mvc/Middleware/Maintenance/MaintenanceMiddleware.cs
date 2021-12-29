using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Mime;

namespace Studens.AspNetCore.Mvc.Middleware.Maintenance;

/// <summary>
/// Middleware that can be used in maintenance cases
/// when we don't want to allow users to access site
/// </summary>
public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MaintenanceOptions _options;

    public MaintenanceMiddleware(
        RequestDelegate next,
        IOptions<MaintenanceOptions> optionsAccessor)
    {
        _next = next;
        _options = optionsAccessor.Value;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        Guard.Against.Null(httpContext, nameof(httpContext));

        var isEnabled = await _options.IsEnabled.Invoke(httpContext);

        if (isEnabled)
        {
            // https://yoast.com/http-503-site-maintenance-seo/
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            httpContext.Response.Headers.Add("Retry-After", _options.RetryAfterSecondsOffset.ToString());
            httpContext.Response.ContentType = MediaTypeNames.Text.Html;

            await httpContext.Response.WriteAsync("Services are currently unavailable.");
        }
        else
        {
            await _next(httpContext);
        }
    }
}

public static class MaintenanceApplicationBuilderExtensions
{
    /// <summary>
    /// Registers <see cref="MaintenanceMiddleware"/> middleware
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMaintenance(this IApplicationBuilder appBuilder)
    {
        return appBuilder.UseMiddleware<MaintenanceMiddleware>();
    }
}