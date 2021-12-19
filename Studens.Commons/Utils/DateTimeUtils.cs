namespace Studens.Commons.Utils;

/// <summary>
/// Additional methods for <see cref="DateTime"/> struct
/// </summary>
public static partial class DateTimeUtils
{
    #region Fields

    /// <summary>
    /// Holds prepared list of day of weeks
    /// </summary>
    private static readonly DayOfWeek[] _dayOfWeeks = Enum.GetValues<DayOfWeek>();

    /// <summary>
    /// Holds prepared list of months
    /// </summary>
    private static readonly Month[] _months = Enum.GetValues<Month>();

    #endregion Fields

    #region Methods

    /// <summary>
    /// Returns all dates between <paramref name="startDate"/> and <paramref name="endDate"/>
    /// exluding start and end dates
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Dates range</returns>
    public static IEnumerable<DateTime> GetDateRangeExclusive(DateTime startDate, DateTime endDate)
    {
        startDate = startDate.AddDays(1);
        for (; startDate.Date < endDate.Date; startDate = startDate.AddDays(1))
        {
            yield return startDate;
        }
    }

    /// <summary>
    /// Returns all dates between <paramref name="startDate"/> and <paramref name="endDate"/>
    /// including start and end dates
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Dates range</returns>
    public static IEnumerable<DateTime> GetDateRangeInclusive(DateTime startDate, DateTime endDate)
    {
        for (; startDate.Date <= endDate.Date; startDate = startDate.AddDays(1))
        {
            yield return startDate;
        }
    }

    /// <summary>
    /// Returns all months for given <paramref name="year"/> and <paramref name="month"/>
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month</param>
    /// <returns>Dates</returns>
    public static IEnumerable<DateTime> GetDatesInMonth(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);

        for (int day = 1; day <= daysInMonth; day++)
        {
            yield return new DateTime(year, month, day);
        }
    }

    /// <summary>
    /// Get all dates for current week
    /// </summary>
    /// <param name="currentDateTime">Current date time</param>
    /// <returns>Dates</returns>
    public static IEnumerable<DateTime> GetDatesInWeek(DateTime currentDateTime)
    {
        var previousDays = currentDateTime.DayOfWeek - DayOfWeek.Monday;
        var startDate = currentDateTime.AddDays(-previousDays);
        var endDate = startDate.AddDays(6);

        return GetDateRangeInclusive(startDate, endDate);
    }

    /// <summary>
    /// Returns all days of the week
    /// </summary>
    public static IEnumerable<DayOfWeek> GetDaysOfWeek(bool startFromMonday = false)
    {
        Func<DayOfWeek, bool> predicate = startFromMonday ?
            (day) => day != DayOfWeek.Sunday :
            (day) => true;

        var daysOfWeek = _dayOfWeeks
            .Where(predicate)
            .ToList();

        if (startFromMonday)
        {
            daysOfWeek.Add(DayOfWeek.Sunday);
        }

        return daysOfWeek;
    }


    /// <summary>
    /// Returns all month names in year
    /// </summary>
    public static IEnumerable<Month> GetMonthsOfYear() => _months;

    #endregion Methods
}