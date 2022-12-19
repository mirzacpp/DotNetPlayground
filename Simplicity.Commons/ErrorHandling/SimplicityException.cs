using System.Runtime.Serialization;

namespace Simplicity.Commons.ErrorHandling;

/// <summary>
/// Represents base exception classs
/// </summary>
public class SimplicityException : Exception
{
    /// <summary>
    /// Exception identifier
    /// </summary>
    public int ExceptionId { get; init; }

    public SimplicityException(int exceptionId, string? message)
        : this(message)
    {
        ExceptionId = exceptionId;
    }

    public SimplicityException(string? message) : base(message)
    {
    }

    public SimplicityException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SimplicityException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}