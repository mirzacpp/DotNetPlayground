using System;
using System.Collections.Generic;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for DateTime formatting
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Fields

        private static readonly Dictionary<DateTimeFormat, string> _formats = new Dictionary<DateTimeFormat, string>
        {
            { DateTimeFormat.Date, "dd.MM.yyyy"},
            { DateTimeFormat.Time, "HH:mm"},
            { DateTimeFormat.DateTime, "dd-MM-yyy HH:mm"},
            { DateTimeFormat.Year, "yyyy" }
        };

        #endregion Fields

        #region Methods

        public static string ToDefaultFormat(this DateTime dateTime)
        {
            return dateTime.ToString(_formats[DateTimeFormat.Date]);
        }

        public static string ToDefaultFormat(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return "/";
            }

            return ((DateTime)dateTime).ToString(_formats[DateTimeFormat.Date]);
        }

        public static string ToFormat(this DateTime dateTime, DateTimeFormat format)
        {
            return dateTime.ToString(_formats[format]);
        }

        public static string ToFormat(this DateTime? dateTime, DateTimeFormat format)
        {
            if (dateTime == null)
            {
                return "/";
            }

            return ((DateTime)dateTime).ToString(_formats[format]);
        }

        public static string ToLocalTimeWithFormat(this DateTime dateTime, DateTimeFormat format = DateTimeFormat.DateTime)
        {
            return dateTime.ToLocalTime().ToString(_formats[format]);
        }

        #endregion Methods
    }

    public enum DateTimeFormat
    {
        Date = 0,
        DateTime = 1,
        Time = 2,
        Year = 3
    }
}