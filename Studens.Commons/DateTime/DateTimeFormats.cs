namespace System;

/// <summary>
/// <see cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings"/>
/// </summary>
public static class DateTimeFormats
{
    public static readonly string ShortDatePattern = "d";
    public static readonly string LongDatePattern = "D";
    public static readonly string FullDateTimePatternShortTime = "f";
    public static readonly string FullDateTimePatternLongTime = "F";
    public static readonly string GeneralDateTimePatternShortTime = "g";
    public static readonly string GeneralDateTimePatternLongTime = "G";
    public static readonly string SortableDateTimePattern = "s";
    public static readonly string ShortTimePattern = "t";
    public static readonly string LongTimePattern = "T";
    public static readonly string UniversalSortableDateTimePattern = "u";
    public static readonly string UniversalFullDateTimePattern = "U";
    public static readonly string YearMonthPattern = "Y";
    public static readonly string YearMonthPatternLowered = "y";
    public static readonly string DayOfWeek = "dddd";
    public static readonly string MonthPattern = "MMMM";
    public static readonly string ShortDateWithShortTime = "MM/dd hh:mm tt";
    public static readonly string MonthDayYear = "MMM dd, yyyy";
}