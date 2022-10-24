using Serilog;
using Simplicity.Templates.Api.Infrastructure;

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.Debug()
.CreateBootstrapLogger();

WebApplication? app = null;

try
{
	Log.Information("Initialising host.");

	var builder = WebApplication
	.CreateBuilder(args)
	.ConfigureHost()
	.ConfigureServices();

	app = builder
	.Build()
	.ConfigurePipeline();

	app.LogApplicationStarted();
	await app.RunAsync();
	app.LogApplicationStopped();

	return 0;
}
catch (Exception ex)
{
	app!.LogApplicationTerminatedUnexpectedly(ex);

	return 1;
}
finally
{
	Log.CloseAndFlush();
}