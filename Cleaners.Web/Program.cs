﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace Cleaners.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Create local configuration for logger configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            // Create and configure logger
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            try
            {
                Log.Information("Starting host.");
                CreateWebHostBuilder(args).Build().Run();

                // Return code 0 on app shutdown
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                // Return code 1 on failed run
                return 1;
            }
            finally
            {
                // Always dispose logger
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// TODO: Refactor to use generic <see cref="Host"/> builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder =>
                {
                    // Clear all default logging providers
                    loggingBuilder.ClearProviders();
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
        }
    }
}