namespace Simplicity.Commons;

/// <summary>
/// Default implementation of <see cref="ITimeZoneProvider"/>
/// </summary>
public class DefaultTimeZoneProvider : ITimeZoneProvider
{
	/// <summary>
	/// Returns UTC as a current time zone.
	/// </summary>
	public Task<TimeZoneInfo> GetCurrentAsync() => Task.FromResult(TimeZoneInfo.Utc);
}