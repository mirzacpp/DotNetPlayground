using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplicity.Swashbuckle.AspNetCore
{
	public static class SwaggerConfigurationExtensions
	{
		/// <summary>
		/// Defines JWT Bearer as any security scheme.
		/// </summary>
		/// <param name="options">Current instance of swagger options</param>
		public static void AddJwtBearerSecurityDefinition(this SwaggerGenOptions options)
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Enter the token value without 'Bearer' prefix.",
				Name = HeaderNames.Authorization,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		}
	}
}