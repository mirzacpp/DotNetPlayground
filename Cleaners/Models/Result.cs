using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Models
{
    /// <summary>
    /// Can be used as a result of any operation
    /// </summary>
    public class Result
    {
        private static readonly Result _succeeded = new Result { Succeeded = true };
        private List<ResultError> _errors = new List<ResultError>();

        public bool Succeeded { get; private set; }
        public IEnumerable<ResultError> Errors => _errors;

        // Indicates that current operation has successful result
        public static Result Success => _succeeded;

        public static Result Failed(params ResultError[] errors)
        {
            var result = new Result { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }

        /// Returns formatted version for result
        public override string ToString()
        {
            return Succeeded ?
                "Succeeded" :
                $"Failed, {Errors.Select(e => e.Message).ToList()}";
        }
    }

    /// <summary>
    /// Represents result error
    /// </summary>
    public class ResultError
    {
        public ResultError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        /// <summary>
        /// Member that this error is associated to
        /// Useful in web scenarion for Model state dictionary
        /// </summary>
        public string Key { get; set; }

        public string Message { get; set; }
    }
}