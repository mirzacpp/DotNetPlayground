using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;

namespace Simplicity.AspNetCore.Mvc.Middleware.ClaimsExplore;

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

        if (user?.Claims.Any() ?? false)
        {
            var claims = JsonSerializer.Serialize(user.Claims.Select(claim => new ClaimsDisplayDto
            {
                Issuer = claim.Issuer,
                OriginalIssuer = claim.OriginalIssuer,
                Type = claim.Type,
                Value = claim.Value,
                ValueType = claim.ValueType
            }).ToList());

            await WriteResponseAsync(claims);
        }
        else
        {
            await WriteResponseAsync("User claims not found.");
        }

        await _next(httpContext);

        // Local method for response write
        async Task WriteResponseAsync(string content)
        {
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsync(content);
        }
    }
}

