using System;

namespace Cleaners.DateTimeUtils
{
    /// <summary>
    /// Date and time abstractions
    /// </summary>
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}