namespace Simplicity.Commons.Result
{
    /// <summary>
    /// Extension methods for <see cref="Result"/>
    /// </summary>
    public static class ResultExtensions
    {
        public static IEnumerable<string> GetErrorMessages(this ErrorResult result) =>
            ExtractErrorMessages(result.Message, result.Errors);

        public static IEnumerable<string> GetErrorMessages<T>(this ErrorResult<T> result) =>
            ExtractErrorMessages(result.Message, result.Errors);

        private static IEnumerable<string> ExtractErrorMessages(string reason, IEnumerable<Error> errors)
        {
            var list = new List<string>
            {
                reason
            };

            list.AddRange(errors.Select(s => s.Details));

            return list;
        }

        public static string GetFlattenedErrorMessages(this ErrorResult result)
        {
            var errors = GetErrorMessages(result);

            return string.Join(Delimiters.Comma, errors);
        }

        public static string GetFlattenedErrorMessages<T>(this ErrorResult<T> result)
        {
            var errors = GetErrorMessages(result);

            return string.Join(Delimiters.Comma, errors);
        }
    }
}