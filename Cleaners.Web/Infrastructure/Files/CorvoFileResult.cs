using System.Collections.Generic;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Represents the result of IO operation
    /// </summary>
    public class CorvoFileResult
    {
        /// <summary>
        /// Returns successfull IO operation
        /// </summary>
        private static CorvoFileResult Success => new CorvoFileResult { Succeded = true };

        public bool Succeded { get; set; }
        public List<CorvoFileError> Errors { get; set; }

        public static CorvoFileResult Error(params CorvoFileError[] errors)
        {
            var result = new CorvoFileResult { Succeded = false };
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }

            return result;
        }
    }
}