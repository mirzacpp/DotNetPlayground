using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace Corvo.AspNetCore.Mvc.Middleware.RegisteredServices
{
    /// <summary>
    /// Middleware that enables list off all registered services inside <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>
    /// Could be useful while developing
    /// </summary>
    /// <see cref="https://github.com/ardalis/AspNetCoreStartupServices"></see>
    public class RegisteredServicesMiddleware
    {
        private readonly RegisteredServicesConfig _config;
        private readonly RequestDelegate _next;

        /// <summary>
        /// By design all arguments inside constructor will be injected as singletons, but in this case <paramref name="config"/>
        /// is already registered as singleton.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="next"></param>
        public RegisteredServicesMiddleware(RegisteredServicesConfig config, RequestDelegate next)
        {
            _config = config;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == _config.Path)
            {
                // Simple HTML markup generation using StringBuilder class
                // For additional styles use embedded styles ?
                var builder = new StringBuilder();
                builder.Append("<h2>Registered services</h2>");

                if (_config.Services.Count > 0)
                {
                    builder.Append($@"<h4 style=""color:red"">There are {_config.Services.Count} registered services.</h4>");
                    builder.Append(@"<table><thead><tr><th>Service type</th><th>Lifetime</th><th>Implementation type</th></tr></thead><tbody>");
                    foreach (var service in _config.Services)
                    {
                        builder.Append("<tr>");
                        builder.Append($"<td>{service.ServiceType.FullName}</td>");
                        builder.Append($"<td>{service.Lifetime}</td>");
                        builder.Append($"<td>{service.ImplementationType?.FullName}</td>");
                        builder.Append("</tr>");
                    }
                    builder.Append("</body></table>");
                }
                else
                {
                    builder.Append("<p>No registered services found.</p>");
                }

                httpContext.Response.ContentType = "text/html";
                await httpContext.Response.WriteAsync(builder.ToString());
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}