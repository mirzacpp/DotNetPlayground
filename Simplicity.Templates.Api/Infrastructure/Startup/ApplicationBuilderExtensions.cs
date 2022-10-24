using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Simplicity.Templates.Api.Infrastructure
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseSwaggerConfigured(this IApplicationBuilder app)
		{
			var apiVersionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

			return app
			.UseSwagger()
			.UseSwaggerUI(options =>
			{
				//Build a swagger endpoint for each discovered API version
				foreach (var description in apiVersionProvider.ApiVersionDescriptions)
				{
					options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
					options.DisplayOperationId();
					options.DisplayRequestDuration();
				}
			});
		}
	}
}