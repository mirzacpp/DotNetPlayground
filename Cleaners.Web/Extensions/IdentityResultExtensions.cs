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
            return errors.Select(e => e.Description);
        }

        /// <summary>
        /// Returns codes for current errors
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetCodes(this IEnumerable<IdentityError> errors)
        {
            return errors.Select(e => e.Code);
        }

        #endregion Methods
    }
}