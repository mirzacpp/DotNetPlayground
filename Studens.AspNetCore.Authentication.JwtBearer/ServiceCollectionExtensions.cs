using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Studens.AspNetCore.Authentication.JwtBearer;
using Studens.AspNetCore.Authentication.JwtBearer.Models;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all services for JWT bearer authentication.
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <returns>Services collection</returns>
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, Action<JwtBearerAuthOptions> configureOptions)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(configureOptions, nameof(configureOptions));

        services.Configure(configureOptions);

        //Bind settings here

        services.Configure<JwtBearerAuthSecurityOptions>(options =>
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(""));
            options.SecurityKey = securityKey;
            options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        });

        services
        .AddSingleton<JwtGenerator>()
        .AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(opt =>
         {
             opt.RequireHttpsMetadata = false;
             opt.SaveToken = true;
             opt.Audience = "Audience";// from opt
             opt.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Secretsecretsecretsecretsecretsecretsecretsecretsecretsecretsecretsecret")),
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 // When token expiration time is being validated it is added on ClockSkew(default 5min. + token expiration time)
                 ClockSkew = TimeSpan.Zero,
                 RequireExpirationTime = true
             };
         });

        return services;
    }
}