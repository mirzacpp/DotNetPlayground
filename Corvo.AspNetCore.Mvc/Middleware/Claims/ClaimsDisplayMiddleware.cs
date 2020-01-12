using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Corvo.AspNetCore.Mvc.Middleware.Claims
{
    /// <summary>
    /// Displays all claims for currently logged in user
    /// </summary>
    public class ClaimsDisplayMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsDisplayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var user = httpContext.User;

            if (user != null)
            {
                var claims = user.Claims;

                if (claims.Any())
                {
                    var builder = new StringBuilder(GetCssStyles());
                    builder.Append(@"<table class=""claims-table"">
                        <thead><tr>
                            <th> Claim type </th>
                            <th> Claim value </th>                            
                        </tr></thead><tbody>");

                    foreach (var claim in claims)
                    {
                        builder.Append("<tr>");
                        builder.Append($"<td>{claim.Type}</td>");
                        builder.Append($"<td>{claim.Value}</td>");                        
                        builder.Append("</tr>");
                    }
                    builder.Append("</tbody></table>");

                    await WriteResponseAsync(builder.ToString());
                }
                else
                {
                    await WriteResponseAsync(@"<h4 style=""color: #17a2b8;"">User doesn't have claims.</h3>");
                }
            }
            else
            {
                await WriteResponseAsync(@"<h4 style=""color: #17a2b8;"">User is not logged in.</h3>");
            }

            await _next(httpContext);

            // Local method for response write
            async Task WriteResponseAsync(string content)
            {
                httpContext.Response.ContentType = MediaTypeNames.Text.Html;
                await httpContext.Response.WriteAsync(content);
            }
        }

        private string GetCssStyles()
        {
            return @"<style>.red {color: red} .claims-table {
                        border: solid 1px #DDEEEE;
                        border-collapse: collapse;
                        border-spacing: 0;
                        font: normal 13px Arial, sans-serif;
                    }
                    .claims-table thead th {
                        background-color: #DDEFEF;
                        border: solid 1px #DDEEEE;
                        color: #336B6B;
                        padding: 10px;
                        text-align: center;
                        text-shadow: 1px 1px 1px #fff;
                    }
                    .claims-table tbody td {
                        border: solid 1px #DDEEEE;
                        color: #333;
                        padding: 10px;
                        text-align: center;
                        text-shadow: 1px 1px 1px #fff;
                    }
                    .text-info {
                        color: #17a2b8;
                    }</style>";
        }
    }
}