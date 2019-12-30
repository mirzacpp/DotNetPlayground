using Microsoft.AspNetCore.Http;
using System.Net.Mime;
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
                builder.Append(GetCssStyles());
                builder.Append(@"<div class=""registered-services"">");
                builder.Append("<h2>Registered services</h2>");

                if (_config.Services.Count > 0)
                {
                    builder.Append($@"<h4 class=""red"">There are {_config.Services.Count} registered services.</h4>");
                    builder.Append(@"<table class=""zui-table"">
                        <thead><tr>
                            <th> Service type </th>
                            <th> Lifetime </th>
                            <th> Implementation type </ th>
                        </tr></thead><tbody>");

                    foreach (var service in _config.Services)
                    {
                        builder.Append("<tr>");
                        builder.Append($"<td>{service.ServiceType.FullName}</td>");
                        builder.Append($"<td>{service.Lifetime}</td>");
                        builder.Append($"<td>{service.ImplementationType?.FullName}</td>");
                        builder.Append("</tr>");
                    }
                    builder.Append("</tbody></table>");
                }
                else
                {
                    builder.Append("<p>No registered services found.</p>");
                }

                httpContext.Response.ContentType = MediaTypeNames.Text.Html;
                await httpContext.Response.WriteAsync(builder.ToString());
            }
            else
            {
                await _next(httpContext);
            }
        }

        private string GetCssStyles()
        {
            return @"<style>
                    .registered-services {
                        font-family: Arial;
                    }
                    .registered-services h4 {
                        color: Arial;
                    }
                    .red {color: red} .zui-table {
                        border: solid 1px #DDEEEE;
                        border-collapse: collapse;
                        border-spacing: 0;
                        font: normal 13px Arial, sans-serif;
                    }
                    .zui-table thead th {
                        background-color: #DDEFEF;
                        border: solid 1px #DDEEEE;
                        color: #336B6B;
                        padding: 10px;
                        text-align: center;
                        text-shadow: 1px 1px 1px #fff;
                    }
                    .zui-table tbody td {
                        border: solid 1px #DDEEEE;
                        color: #333;
                        padding: 10px;
                        text-align: center;
                        text-shadow: 1px 1px 1px #fff;
                    }</style>";
        }
    }
}