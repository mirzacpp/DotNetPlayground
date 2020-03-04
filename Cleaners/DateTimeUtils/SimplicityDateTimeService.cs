using System;

namespace Cleaners.DateTimeUtils
{
    /// <summary>
    /// Default implementation of <see cref="IDateTimeService"/>
    /// </summary>
    public class SimplicityDateTimeService : IDateTimeService
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        // TimeZoneInfo can be set at service startup registration or move it to separate service if
        // timezone can be set by user
        public SimplicityDateTimeService(TimeZoneInfo timeZoneInfo)
        {
            _timeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Utc;
        }

        public DateTime DateUtcNow => DateTime.UtcNow;

        public DateTime DateNow => ConvertToLocal(DateUtcNow);

        private DateTime ConvertToLocal(DateTime utc)
            => TimeZoneInfo.ConvertTimeFromUtc(utc, _timeZoneInfo);

        private DateTime? ConvertToLocal(DateTime? utc)
            => !utc.HasValue ? (DateTime?)null : ConvertToLocal(utc.Value);
    }
}