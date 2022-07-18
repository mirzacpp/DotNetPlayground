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

        // We will build options manually
        JwtBearerAuthOptions authOptions = new();
        configureOptions(authOptions);        

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Secret));
        var securityOptions = new JwtBearerAuthSecurityOptions
        {
            SecurityKey = securityKey,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        };

        services.Configure<JwtBearerAuthSecurityOptions>(options =>
        {            
            options.SecurityKey = securityOptions.SecurityKey;
            options.SigningCredentials = securityOptions.SigningCredentials;
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
             opt.RequireHttpsMetadata = false; // False should be only in dev ...
             opt.SaveToken = true;
             opt.Audience = authOptions.Audience;
             opt.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = securityOptions.SecurityKey,
                 ValidateIssuer = !string.IsNullOrEmpty(authOptions.Issuer),
                 ValidIssuer = authOptions.Issuer,
                 ValidateAudience = !string.IsNullOrEmpty(authOptions.Audience),
                 ValidAudience = authOptions.Audience,
                 ValidateLifetime = true,
                 // When token expiration time is being validated it is added on ClockSkew(default 5min. + token expiration time)
                 ClockSkew = TimeSpan.Zero,
                 RequireExpirationTime = true,
             };
         });

        return services;
    }
}