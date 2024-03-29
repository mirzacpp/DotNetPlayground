﻿using System.Runtime.InteropServices;

using Serilog;
using Serilog.Formatting.Compact;

using Simplicity.Templates.Api.Configuration;

namespace Simplicity.Templates.Api.Infrastructure;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Configures host builder
    /// </summary>
    /// <param name="builder">Current instance of host builder</param>
    /// <returns>The same instance of the <see cref="WebApplicationBuilder"/> for chaining.</returns>
    public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
    {
        builder.Host.UseDefaultServiceProvider((context, options) =>
        {
            var isDevelopment = context.HostingEnvironment.IsDevelopment();

            options.ValidateScopes = isDevelopment;
            options.ValidateOnBuild = isDevelopment;
        });

        builder.Logging.ClearProviders();
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

    /// <summary>
    /// TODO: Add antiforgery token for SPA 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services
        .AddPocoOptions<ApplicationOptions>(nameof(ApplicationOptions), builder.Configuration, out var appOptions)
        .AddApiVersioningConfigured()
        //.AddCors()
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        .AddIf(appOptions.Setup.EnableOpenApi, services => services.AddSwaggerConfigured())
        .AddIf(appOptions.Setup.EnableMiniProfiler, services => services.AddMiniProfilerConfigured())
        .AddHttpContextAccessor();

        builder.Services.AddHealthChecks();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        var appOptions = app.Services.GetRequiredService<ApplicationOptions>();

        app
        .UseIf(
            appOptions.Setup.LogConfigurationFile,
            app =>
            {
                var webApp = (WebApplication)app;
                webApp.Logger.LogDebug(((IConfigurationRoot)webApp.Configuration).GetDebugView());
                return app;
            })
        .UseIf(appOptions.Setup.EnableOpenApi, app => app.UseSwaggerConfigured())
        .UseIf(appOptions.Setup.EnableMiniProfiler, app => app.UseMiniProfiler())
        .UseRouting()
        .UseIf(app.Environment.IsDevelopment(), app => app.UseDeveloperExceptionPage())
        //.UseStaticFiles()
        //.UseCors()
        //.UseAuthentication()
        .UseIf(appOptions.Setup.EnableHttpLogging, app => app.UseSerilogRequestLogging())
        .UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/status");
        //Map "/" request to redirect to swagger endpoint
        if (appOptions.Setup.EnableOpenApi)
        {
            app.MapGet("/", (context) =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        }

        return app;
    }

    /// <summary>
    /// Logs application started event
    /// </summary>
    /// <param name="app">Current host</param>
    public static void LogApplicationStarted(this WebApplication app)
    {
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
    /// <param name="app">Current host</param>
    public static void LogApplicationStopped(this WebApplication app)
    {
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
    /// <param name="app">Current host</param>
    public static void LogApplicationTerminatedUnexpectedly(this WebApplication app, Exception exception)
    {
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
            catch
            {
                LogToConsole(exception);
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