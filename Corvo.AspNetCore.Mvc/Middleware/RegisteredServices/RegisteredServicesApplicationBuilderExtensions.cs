using Microsoft.AspNetCore.Builder;
using System;

namespace Corvo.AspNetCore.Mvc.Middleware.RegisteredServices
{
    public static class RegisteredServicesApplicationBuilderExtensions
    {
        public static void UseShowRegisteredServices(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<RegisteredServicesMiddleware>();
        }
    }
}