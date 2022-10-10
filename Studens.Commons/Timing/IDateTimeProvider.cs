namespace Studens.Commons;

/// <summary>
/// Defines contract for date-time operations.
/// </summary>
public interface IDateTimeProvider
{
	bool TimeZoneExists(string timeZoneId);

	DateTime GetByTimeZone(string timeZoneId);

	DateTime ConvertToUtc(DateTime dateTime, string timeZoneId);

	/// <summary>
	/// Converts given time to using specified <paramref name="timeZoneId"/>.
	/// </summary>
	/// <param name="dateTime">Date time to convert</param>
	/// <param name="timeZoneId">Timezone id</param>
	/// <returns>Converted date time</returns>
	DateTime ConvertToLocal(DateTime dateTime, string? timeZoneId);

	bool IsInvalidTime(DateTime dateTime, string timeZoneId);

	bool IsAmbiguousTime(DateTime dateTime, string timeZoneId);
}