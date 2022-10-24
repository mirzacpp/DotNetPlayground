namespace Simplicity.Templates.WebUI;

/// <summary>
/// Extension methods for <see cref = "ILogger" />
/// For more info on LoggerMessage check the docs https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator.
/// </summary>
internal static partial class LoggingExtensions
{
    [LoggerMessage(
        EventId = 5000,
        Level = LogLevel.Information,
        Message = "Started {Application} in {Environment} mode with runtime {Runtime} and OS {OperatingSystem}.")]
    public static partial void ApplicationStarted(this ILogger logger,
    string application,
    string environment,
    string runtime,
    string operatingSystem);

    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "Stopped {Application} in {Environment} mode with runtime {Runtime} and OS {OperatingSystem}.")]
    public static partial void ApplicationStopped(
           this ILogger logger,
           string application,
           string environment,
           string runtime,
           string operatingSystem);
    
    [LoggerMessage(
           EventId = 5002,
           Level = LogLevel.Critical,
           Message = "{Application} terminated unexpectedly in {Environment} mode with runtime {Runtime} and OS {OperatingSystem}.")]
    public static partial void ApplicationTerminatedUnexpectedly(
           this ILogger logger,
           Exception exception,
           string application,
           string environment,
           string runtime,
           string operatingSystem);
}