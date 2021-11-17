namespace Studens.Commons.Result
{
    /// <summary>
    /// Base Result class used for app flow control
    /// Credits to https://josef.codes/my-take-on-the-result-class-in-c-sharp/
    /// </summary>
    public abstract class Result
    {
        public bool Success { get; protected set; }
        public bool Failure => !Success;
    }

    /// <summary>
    /// Generic extension on <see cref="Result"/>
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public abstract class Result<T> : Result
    {
        private T _data;

        protected Result(T data) => Data = data;

        public T Data
        {
            get => Success ? _data : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
            set => _data = value;
        }
    }

    /// <summary>
    /// Basic success result
    /// </summary>
    public class SuccessResult : Result
    {
        public SuccessResult()
        {
            Success = true;
        }
    }

    /// <summary>
    /// Basic generic success result
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T data) : base(data)
        {
            Success = true;
        }
    }

    /// <summary>
    /// Basic error result
    /// </summary>
    public class ErrorResult : Result, IErrorResult
    {
        public ErrorResult(string message)
            : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }
    }

    /// <summary>
    /// Basic Generic error result so we can respect method signature.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class ErrorResult<T> : Result<T>, IErrorResult
    {
        public ErrorResult(string message)
            : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors) 
            : base(default)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; set; }
        public IReadOnlyCollection<Error> Errors { get; }
    }

    public class Error
    {
        public Error(string details) : this(string.Empty, details)
        {
        }

        public Error(string code, string details)
        {
            Code = code;
            Details = details;
        }

        public string Code { get; }
        public string Details { get; }
    }

    internal interface IErrorResult
    {
        string Message { get; }
        IReadOnlyCollection<Error> Errors { get; }
    }
}

