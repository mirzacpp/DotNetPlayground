using Ardalis.GuardClauses;
using Cleaners.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.AspNetCore.Identity.IdentityError"/>
    /// </summary>
    public static class IdentityResultExtensions
    {
        #region Methods

        /// <summary>
        /// Returns descriptions for current errors
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDescriptions(this IEnumerable<IdentityError> errors)
        {
            Guard.Against.Null(errors, nameof(errors));

            return errors.Select(e => e.Description).ToList();
        }

        /// <summary>
        /// Returns codes for current errors
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetCodes(this IEnumerable<IdentityError> errors)
        {
            Guard.Against.Null(errors, nameof(errors));

            return errors.Select(e => e.Code).ToList();
        }

        /// <summary>
        /// Converts <see cref="IdentityResult"/> to <see cref="Result"/>
        /// </summary>
        /// <param name="identityResult"></param>
        /// <returns></returns>
        public static Result ToApplicationResult(this IdentityResult identityResult)
        {
            Guard.Against.Null(identityResult, nameof(identityResult));

            return identityResult.Succeeded ?
                Result.Success :
                Result.Failed(identityResult.Errors.Select(e => new ResultError(string.Empty, e.Description)).ToArray());
        }

        #endregion Methods
    }
}