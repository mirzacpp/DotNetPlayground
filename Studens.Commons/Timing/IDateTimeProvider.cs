namespace Studens.Commons;

public interface IDateTimeProvider
{
    Task<DateTime> GetUtcDateTimeAsync();

    Task<DateOnly> GetUtcDateAsync();

    Task<TimeOnly> GetUtcTimeAsync();

    Task<DateTime> GetLocalDateTimeAsync();

    Task<DateOnly> GetLocalDateAsync();

    Task<TimeOnly> GetLocalTimeAsync();
}

