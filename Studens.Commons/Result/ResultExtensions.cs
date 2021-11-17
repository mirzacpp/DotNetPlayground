namespace Studens.Commons.Result
{
    /// <summary>
    /// Extension methods for <see cref="Result"/>
    /// </summary>
    public static class ResultExtensions
    {
        public static IEnumerable<string> GetPreparedErrorMessages(this ErrorResult result)
        {
            var errors = result.Errors.Select(s => s.Details).ToList();
            errors.Add(result.Message);

            return errors;
        }

        public static IEnumerable<string> GetPreparedErrorMessages<T>(this ErrorResult<T> result)
        {
            var errors = result.Errors.Select(s => s.Details).ToList();
            errors.Add(result.Message);

            return errors;
        }

        public static string GetFlattenedErrorMessages(this ErrorResult result)
        {
            var errors = GetPreparedErrorMessages(result);

            return string.Join(Delimiters.Comma, errors);
        }

        public static string GetFlattenedErrorMessages<T>(this ErrorResult<T> result)
        {
            var errors = GetPreparedErrorMessages(result);

            return string.Join(Delimiters.Comma, errors);
        }
    }
}
