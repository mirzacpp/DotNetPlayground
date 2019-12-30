using System;

namespace Cleaners.DateTimeUtils
{
    /// <summary>
    /// Default implementation of <see cref="IDateTimeService"/>
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}