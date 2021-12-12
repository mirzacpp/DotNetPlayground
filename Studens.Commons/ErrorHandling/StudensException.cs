using System.Runtime.Serialization;

namespace Studens.Commons.ErrorHandling;

/// <summary>
/// Represents base exception class
/// </summary>
public class StudensException : Exception
{
    /// <summary>
    /// Exception identifier
    /// </summary>
    public int ExceptionId { get; init; }

    public StudensException(int exceptionId, string? message)
        : this(message)
    {
        ExceptionId = exceptionId;
    }

    public StudensException(string? message) : base(message)
    {
    }

    public StudensException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected StudensException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Base ToString override
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}