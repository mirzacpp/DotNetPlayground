using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Simplicity.Api.Infrastructure.Versioning;
using Simplicity.Swashbuckle.AspNetCore;
using Simplicity.Swashbuckle.AspNetCore.OperationFilters;
using StackExchange.Profiling;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Simplicity.Api.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSwaggerConfigured(this IServiceCollection services)
		{
			return services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(options =>
			{
				//options.SwaggerDoc("")	

				options.OperationFilter<AcceptLanguageHeader>();
				options.AddJwtBearerSecurityDefinition();

				var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
			});
		}

		public static IServiceCollection AddMiniProfilerConfigured(this IServiceCollection services)
		{
			services.AddMiniProfiler(options =>
			{
				options.EnableServerTimingHeader = true;
				options.RouteBasePath = "/profiler";
				options.EnableDebugMode = false;
				options.ColorScheme = ColorScheme.Light;
				options.EnableMvcViewProfiling = false;
			});

			return services;
		}

		public static IServiceCollection AddApiVersioningConfigured(this IServiceCollection services)
		{
			services
			.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
			.AddApiVersioning(options =>
			{
				// In case consumer does not specify version, fallback to default one
				options.AssumeDefaultVersionWhenUnspecified = true;
				// Set V1.0 as a default version
				options.DefaultApiVersion = ApiVersion.Default;
				// Returns list of supported version in response
				options.ReportApiVersions = true;

				options.ApiVersionReader = ApiVersionReader.Combine(
				new UrlSegmentApiVersionReader(),
				//new QueryStringApiVersionReader("v"),
				new HeaderApiVersionReader("X-Api-Version"));
			})
			.AddVersionedApiExplorer(options =>
			{
				// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
				// note: the specified format code will format the version as "'v'major[.minor][-status]"
				options.GroupNameFormat = "'v'VVV";
			});

			return services;
		}
	}
}