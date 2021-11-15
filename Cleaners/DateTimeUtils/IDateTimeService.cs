using System;

namespace Cleaners.DateTimeUtils
{
    /// <summary>
    /// Date/Time provider
    /// </summary>
    public interface IDateTimeService
    {
        DateTime DateNow { get; }

        DateTime DateUtcNow { get; }        
    }
}