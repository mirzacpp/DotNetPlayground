using System;
using System.Collections.Generic;

namespace Cleaners.Utils
{
    /// <summary>
    /// Additional methods for <see cref="System.DateTime"/> struct
    /// </summary>
    public static class DateTimeUtils
    {
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
        public static IEnumerable<DateTime> GetMonthDates(int year, int month)
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
        public static IEnumerable<DateTime> GetWeekDates(DateTime currentDateTime)
        {
            var previousDays = currentDateTime.DayOfWeek - DayOfWeek.Monday;
            var startDate = currentDateTime.AddDays(-previousDays);
            var endDate = startDate.AddDays(6);

            return GetDateRangeInclusive(startDate, endDate);
        }

        /// <summary>
        /// Returns all day names in week
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DayOfWeek> GetDaysOfWeek()
        {
            return EnumUtils.GetEnumNames<DayOfWeek>();
        }

        /// <summary>
        /// Returns all month names in year
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Month> GetMonthsOfYear()
        {
            return EnumUtils.GetEnumNames<Month>();
        }

        #region Month of year enum

        /// <summary>
        /// .NET doesn't provide built in Month name enum
        /// </summary>
        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        #endregion Month of year enum
    }
}