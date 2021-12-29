using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Studens.AspNetCore.Mvc.Middleware.RegisteredServices;

/// <summary>
/// Middleware that enables list off all registered services inside <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>
/// Can be useful while developing
/// </summary>
/// <see cref="https://github.com/ardalis/AspNetCoreStartupServices"></see>
public class RegisteredServicesMiddleware
{
    private readonly RegisteredServicesOptions _options;
    private readonly RequestDelegate _next;

    /// <summary>
    /// By design all arguments inside constructor will be injected as singletons, but in this case <paramref name="config"/>
    /// is already registered as singleton.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="next"></param>
    public RegisteredServicesMiddleware(IOptions<RegisteredServicesOptions> optionsAccessor, RequestDelegate next)
    {
        _options = optionsAccessor.Value;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Path == _options.DefaultPath && _options.Services.Count > 0)
        {
            var builder = new StringBuilder();
            builder.Append("<h1>Registered services</h2>" + _options.Services.Count.ToString());

            builder.Append(@"<table>
                        <thead><tr>
                            <th>Service type</th>
                            <th>Implementation type</th>
                            <th>Lifetime</th>
                        </tr></thead><tbody>");
            foreach (var service in _options.Services)
            {
                builder.Append("<tr>");
                builder.Append($"<td>{service.ServiceType.FullName}</td>");
                builder.Append($"<td>{service.ImplementationType?.FullName}</td>");
                builder.Append($"<td>{service.Lifetime}</td>");
                builder.Append("</tr>");
            }
            builder.Append("</tbody></table>");
            
            httpContext.Response.ContentType = MediaTypeNames.Text.Html;            
            await httpContext.Response.WriteAsync(builder.ToString());
        }
        else
        {
            await _next(httpContext);
        }
    }
}

/// <summary>
/// Extension methods for middleware configuration
/// </summary>
public static class RegisteredServicesApplicationBuilderExtensions
{
    public static void UseDisplayRegisteredServices(this IApplicationBuilder app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.UseMiddleware<RegisteredServicesMiddleware>();
    }
}