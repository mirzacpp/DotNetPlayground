using System.Runtime.InteropServices;

namespace Simplicity.Templates.Api.Infrastructure.Startup;

/// <summary>
/// Logging extensions for <see cref="IHost"/>
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Logs application started event
	/// </summary>
	/// <param name="host">Current host</param>
	public static void LogApplicationStarted(this IHost host)
	{
		Guard.Against.Null(host, nameof(host));

		var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
		var logger = host.Services.GetRequiredService<ILogger<Program>>();

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
	public static void LogApplicationStopped(this IHost host)
	{
		Guard.Against.Null(host, nameof(host));

		var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
		var logger = host.Services.GetRequiredService<ILogger<Program>>();

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
	public static void LogApplicationTerminatedUnexpectedly(this IHost host, Exception exception)
	{
		Guard.Against.Null(host, nameof(host));

		// Check if host is null
		if (host == null)
		{
			LogToConsole(exception);
		}
		else
		{
			// We have to wrap inside try/catch because some of the services could be disposed
			try
			{
				var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
				var logger = host.Services.GetRequiredService<ILogger<Program>>();

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
	/// <param name="exception"></param>
	private static void LogToConsole(Exception exception)
	{
		var foregroundColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"{AssemblyInformation.Current.Product} terminated unexpectedly.");
		Console.WriteLine(exception.ToString());
		Console.ForegroundColor = foregroundColor;
	}
}