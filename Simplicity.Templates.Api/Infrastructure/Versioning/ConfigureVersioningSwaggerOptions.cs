using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplicity.Templates.Api.Infrastructure.Versioning;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
public class ConfigureVersioningSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
	private readonly IApiVersionDescriptionProvider _provider;	

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigureVersioningSwaggerOptions"/> class.
	/// </summary>
	/// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
	public ConfigureVersioningSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

	/// <inheritdoc />
	public void Configure(SwaggerGenOptions options)
	{
		// add a swagger document for each discovered API version
		// note: you might choose to skip or document deprecated API versions differently
		foreach (var description in _provider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
		}
	}

	private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
	{
		var info = new OpenApiInfo()
		{
			Title = "Simplicity Api Template",
			Description = "Simplicity Api Template.",
			Version = description.ApiVersion.ToString(),
			Contact = new OpenApiContact {
				Email = "contact@email.com"
			}
		};

		if (description.IsDeprecated)
		{
			info.Description += " This API version has been deprecated.";
		}

		return info;
	}
}