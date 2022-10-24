using Ardalis.GuardClauses;
using Serilog;
using Serilog.Formatting.Compact;
using Simplicity.Api.Configuration;
using System.Runtime.InteropServices;

namespace Simplicity.Api.Infrastructure;

public static class WebApplicationBuilderExtensions
{
	/// <summary>
	/// Configures host builder
	/// </summary>
	/// <param name="builder">Current instance of host builder</param>
	/// <returns>The same instance of the <see cref="WebApplicationBuilder"/> for chaining.</returns>
	public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
	{
		builder.Host.ConfigureLogging((context, options) =>
		{
			options.ClearProviders();
		})
		.UseDefaultServiceProvider((context, options) =>
		{
			var isDevelopment = context.HostingEnvironment.IsDevelopment();

			options.ValidateScopes = isDevelopment;
			options.ValidateOnBuild = isDevelopment;
		});

		builder.Host.UseSerilog((context, services, configuration) =>
		{
			configuration
			.ReadFrom.Configuration(context.Configuration)
			.ReadFrom.Services(services)
			.Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
			.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
			// TODO: For PRODUCTION, log to file or some third pary service (Seq, AppInsights...)
			.WriteTo.Conditional(
				c => context.HostingEnvironment.IsDevelopment(),
				sink => sink.Notepad(formatter: new RenderedCompactJsonFormatter()).WriteTo.Debug())
			.WriteTo.Conditional(
				c => context.HostingEnvironment.IsDevelopment(),
				sink => sink.Console(formatter: new RenderedCompactJsonFormatter()).WriteTo.Debug());
		})
		.UseConsoleLifetime();

		builder.WebHost.ConfigureKestrel(options =>
		{
			options.AddServerHeader = false;
		});

		return builder;
	}

	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllers();
		builder.Services
		.AddPocoOptions<ApplicationOptions>(nameof(ApplicationOptions), builder.Configuration, out var appOptions)
		.AddApiVersioningConfigured()
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		.AddIf(appOptions.Setup.EnableOpenApi, services => services.AddSwaggerConfigured())
		.AddIf(appOptions.Setup.EnableMiniProfiler, services => services.AddMiniProfilerConfigured())
		.AddHttpContextAccessor();

		return builder;
	}

	public static WebApplication ConfigurePipeline(this WebApplication app)
	{
		var appOptions = app.Services.GetRequiredService<ApplicationOptions>();
		// Configure the HTTP request pipeline.
		app
		.UseIf(appOptions.Setup.EnableOpenApi, app => app.UseSwaggerConfigured())
		.UseIf(appOptions.Setup.EnableHttpsRedirection, app => app.UseHttpsRedirection())
		.UseIf(appOptions.Setup.EnableMiniProfiler, app => app.UseMiniProfiler())
		.UseAuthorization();

		//app.MapControllers();

		//Map "/" request to redirect to swagger endpoint
		app.MapGet("/", (context) =>
		{
			context.Response.Redirect("/swagger");
			return Task.CompletedTask;
		});

		return app;
	}

	/// <summary>
	/// Logs application started event
	/// </summary>
	/// <param name="host">Current host</param>
	public static void LogApplicationStarted(this WebApplication app)
	{
		Guard.Against.Null(app, nameof(app));

		var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
		var logger = app.Services.GetRequiredService<ILogger<Program>>();

		logger.ApplicationStarted(
			hostEnvironment.ApplicationName,
			hostEnvironment.EnvironmentName,
			RuntimeInformation.FrameworkDescription,
			RuntimeInformation.OSDescription);
	}

	/// <summary>
	/// Logs application stopped event
	/// </summary>
	/// <param name="host">Current host</param>
	public static void LogApplicationStopped(this WebApplication app)
	{
		Guard.Against.Null(app, nameof(app));

		var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
		var logger = app.Services.GetRequiredService<ILogger<Program>>();

		logger.ApplicationStopped(
			hostEnvironment.ApplicationName,
			hostEnvironment.EnvironmentName,
			RuntimeInformation.FrameworkDescription,
			RuntimeInformation.OSDescription);
	}

	/// <summary>
	/// Logs application terminated unexpectedly
	/// </summary>
	/// <param name="host">Current host</param>
	public static void LogApplicationTerminatedUnexpectedly(this WebApplication app, Exception exception)
	{
		Guard.Against.Null(app, nameof(app));

		// Check if host is null
		if (app == null)
		{
			LogToConsole(exception);
		}
		else
		{
			// We have to wrap inside try/catch because some of the services could be disposed
			try
			{
				var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
				var logger = app.Services.GetRequiredService<ILogger<Program>>();

				logger.ApplicationTerminatedUnexpectedly(exception,
					hostEnvironment.ApplicationName,
					hostEnvironment.EnvironmentName,
					RuntimeInformation.FrameworkDescription,
					RuntimeInformation.OSDescription);
			}
			catch (Exception ex)
			{
				LogToConsole(ex);
			}
		}
	}

	/// <summary>
	/// Log to console fallback
	/// </summary>
	/// <param name="exception">Exception to log</param>
	private static void LogToConsole(Exception exception)
	{
		var foregroundColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"{AssemblyInformation.Current.Product} terminated unexpectedly.");
		Console.WriteLine(exception.ToString());
		Console.ForegroundColor = foregroundColor;
	}
}