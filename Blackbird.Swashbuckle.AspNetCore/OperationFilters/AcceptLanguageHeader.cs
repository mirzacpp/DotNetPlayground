using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplicity.Swashbuckle.AspNetCore.OperationFilters
{
	/// <summary>
	/// Enables accept-language header
	/// </summary>
	public class AcceptLanguageHeader : IOperationFilter
	{
		/// <summary>
		/// Applies the filter to the specified operation using the given context.
		/// </summary>
		/// <param name="operation">The operation to apply the filter to.</param>
		/// <param name="context">The current operation filter context.</param>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Parameters ??= new List<OpenApiParameter>();

			operation.Parameters.Add(new OpenApiParameter
			{
				Name = HeaderNames.AcceptLanguage,
				In = ParameterLocation.Header,
				Required = false,
				Schema = new OpenApiSchema
				{
					Type = "String"
				}
			});
		}
	}
}