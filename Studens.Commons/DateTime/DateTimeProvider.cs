namespace Studens.Commons;

public class DateTimeProvider : IDateTimeProvider
{
    private readonly ITimeZoneProvider _timeZoneProvider;

    public DateTimeProvider(ITimeZoneProvider timeZoneProvider)
    {
        _timeZoneProvider = timeZoneProvider;
    }

    public Task<DateTime> GetUtcDateTimeAsync() => Task.FromResult(DateTime.UtcNow);

    public async Task<DateOnly> GetUtcDateAsync()
    {
        var utcNow = await GetUtcDateTimeAsync();

        return DateOnly.FromDateTime(utcNow);
    }

    public async Task<TimeOnly> GetUtcTimeAsync()
    {
        var utcNow = await GetUtcDateTimeAsync();

        return TimeOnly.FromDateTime(utcNow);
    }

    public async Task<DateTime> GetLocalDateTimeAsync()
    {
        var utcNow = await GetUtcDateTimeAsync();

        return await ConvertToLocalAsync(utcNow);
    }

    public async Task<DateOnly> GetLocalDateAsync()
    {
        var local = await ConvertToLocalAsync(await GetUtcDateTimeAsync());

        return DateOnly.FromDateTime(local);
    }

    public async Task<TimeOnly> GetLocalTimeAsync()
    {
        var local = await ConvertToLocalAsync(await GetUtcDateTimeAsync());

        return TimeOnly.FromDateTime(local);
    }

    #region Utils

    /// <summary>
    /// Converts given UTC time to local time
    /// </summary>        
    private async Task<DateTime> ConvertToLocalAsync(DateTime utcTime)
    {
        var timeZoneInfo = await _timeZoneProvider.GetCurrentAsync();

        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
    }

    #endregion
}

