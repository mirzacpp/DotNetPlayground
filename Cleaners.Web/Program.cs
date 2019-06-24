using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace Cleaners.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = CreateConfigurationBuilder(environment).Build();

            // Configure logger from appsettings.json file
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            var host = CreateWebHostBuilder(args).Build();

            host.Run();
        }

        public static IConfigurationBuilder CreateConfigurationBuilder(string environment) =>
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureKestrel(options => options.AddServerHeader = false)            
            .ConfigureLogging((hostContext, config) =>
            {
                // Remove default logger providers
                config.ClearProviders();
            })
            // Register serilog as logging provider
            .UseSerilog()
            .UseStartup<Startup>();
    }
}