namespace Simplicity.Commons;

/// <summary>
/// Abstractions for time zone provider 
/// </summary>
public interface ITimeZoneProvider
{
    Task<TimeZoneInfo> GetCurrentAsync();
}

