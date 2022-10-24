using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Formatting.Compact;

namespace Simplicity.Templates.WebUI;

/// <summary>
/// TODO: Add Serilog
/// </summary>
public class Program
{
	public static async Task<int> Main(string[] args)
	{
		Log.Logger = CreateBootstrapLogger();

		IHost? host = null;

		try
		{
			Log.Information("Initialising host.");

			host = CreateHostBuilder(args).Build();

			host.LogApplicationStarted();
			await host.RunAsync();
			host.LogApplicationStopped();

			return 0;
		}
		catch (Exception ex)
		{
			host!.LogApplicationTerminatedUnexpectedly(ex);

			return 1;
		}
		finally
		{
			// Flush logger
			Log.CloseAndFlush();
		}
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		   Host.CreateDefaultBuilder(args)
				.ConfigureLogging((context, options) =>
				{
					options.ClearProviders();
				})
			   .UseDefaultServiceProvider((context, options) =>
			   {
				   var isDevelopment = context.HostingEnvironment.IsDevelopment();

				   options.ValidateScopes = isDevelopment;
				   options.ValidateOnBuild = isDevelopment;
			   })
			   .UseSerilog((context, services, configuration) =>
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
			   .ConfigureWebHostDefaults(webBuilder =>
			   {
				   webBuilder.UseStartup<Startup>();
				   webBuilder.UseKestrel(options => options.AddServerHeader = false);
			   })
			   .UseConsoleLifetime();

	#region Logging utils

	/// <summary>
	/// Creates a logger used during application initialisation.
	/// <see href="https://nblumhardt.com/2020/10/bootstrap-logger/"/>.
	/// </summary>
	/// <returns>A logger that can load a new configuration.</returns>
	private static ReloadableLogger CreateBootstrapLogger() =>
		new LoggerConfiguration()
			.WriteTo.Console()
			.WriteTo.Debug()
			.CreateBootstrapLogger();

	#endregion Logging utils
}