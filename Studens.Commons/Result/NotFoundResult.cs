namespace Studens.Commons.Result
{
    /// <summary>
    /// Not found result implementation
    /// </summary>
    public class NotFoundResult : ErrorResult
    {
        /// <summary>
        /// Additional info on missing resource
        /// </summary>
        public object? Id { get; set; }

        public NotFoundResult() : base($"Could not find resource.")
        {
        }

        public NotFoundResult(object id, string message) : base(message)
        {
            Id = id;
        }

        public NotFoundResult(object id) : base($"Could not find resource with id {id}")
        {
            Id = id;
        }
    }

    /// <summary>
    /// Not found result generic implementation
    /// </summary>
    public class NotFoundResult<T> : ErrorResult<T>
    {
        /// <summary>
        /// Additional info on missing resource
        /// </summary>
        public object Id { get; set; }

        public NotFoundResult() : base($"Could not find resource.")
        {
        }

        public NotFoundResult(object id, string message) : base(message)
        {
            Id = id;
        }

        public NotFoundResult(object id) : base($"Could not find resource of type {typeof(T)} with id {id}")
        {
            Id = id;
        }
    }
}