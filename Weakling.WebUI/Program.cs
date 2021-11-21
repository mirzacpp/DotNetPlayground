using Serilog;
using Serilog.Extensions.Hosting;

namespace Weakling.WebUI;

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
            Log.Information("Initialising.");

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
               .UseDefaultServiceProvider((context, options) =>
               {
                   var isDevelopment = context.HostingEnvironment.IsDevelopment();

                   options.ValidateScopes = isDevelopment;
                   options.ValidateOnBuild = isDevelopment;
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

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