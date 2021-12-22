using Microsoft.Extensions.Logging;

namespace Studens.AspNetCore.Identity;

internal static class LoggerEventIds
{
    public static readonly EventId UserNotInRole = new EventId(6, "UserNotInRole");
}