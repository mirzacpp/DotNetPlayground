using System;
using System.Collections.Generic;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.TimeSpan"/>
    /// Check https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings for more info
    /// </summary>
    /// <remarks>
    /// Check Humanizer library at GitHub
    /// </remarks>
    public static class TimeSpanExtensions
    {
        #region Fields

        private static readonly Dictionary<TimeSpanFormat, string> _formats = new Dictionary<TimeSpanFormat, string>
        {
            { TimeSpanFormat.DayHourMinuteSecond, "{0:%d}d / {0:%h}h / {0:%m}m / {0:%s}s" },
            { TimeSpanFormat.HourMinuteSecond, "{0:%h}h / {0:%m}m / {0:%s}s" }
        };

        #endregion Fields

        #region Methods

        /// <summary>
        /// Formats time span using specified or default format
        /// </summary>
        public static string Format(this TimeSpan timeSpan, TimeSpanFormat format = TimeSpanFormat.DayHourMinuteSecond)
        {
            return string.Format(_formats[format], timeSpan);
        }

        #endregion Methods

        #region Formats

        public enum TimeSpanFormat
        {
            DayHourMinuteSecond = 0,
            HourMinuteSecond = 1,
        }

        #endregion Formats
    }
}