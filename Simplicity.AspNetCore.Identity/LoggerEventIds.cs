namespace Simplicity.AspNetCore.Identity;

/// <summary>
/// Make sure we start with a number greater than 15 so we do not conflict with internal Identity event ids
/// </summary>
internal static class LoggerEventIds
{
    public static readonly EventId UserAlreadyInRole = new(20, "UserAlreadyInRole");

    /// <summary>
    /// This event already exists internally so keep the value at 6
    /// </summary>
    public static readonly EventId UserNotInRole = new(6, "UserNotInRole");
}