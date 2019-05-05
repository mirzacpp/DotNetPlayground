using System;
using System.Collections.Generic;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.TimeSpan"/>
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Fields

        private static readonly Dictionary<TimeSpanFormat, string> _formats = new Dictionary<TimeSpanFormat, string>
        {
            { TimeSpanFormat.DayHourMinuteSecond, "{0:%d}d / {0:%h}h / {0:%m}m / {0:%s}s" }
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
        }

        #endregion Formats
    }
}