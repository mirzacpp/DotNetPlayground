using Ardalis.GuardClauses;
using Cleaners.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Cleaners.Web.Extensions
{
    /// <summary>
    /// Extends ModelStateDictionary methods
    /// </summary>
    /// <remarks>
    /// MOVE TO AspNetCore.Mvc project
    /// </remarks>
    public static class ModelStateExtensions
    {
        #region Methods

        public static void AddModelErrors(this ModelStateDictionary modelState, IDictionary<string, string> errors)
        {
            Guard.Against.Null(modelState, nameof(modelState));
            Guard.Against.Null(errors, nameof(errors));

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }
        }

        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<string> errors)
        {
            Guard.Against.Null(modelState, nameof(modelState));
            Guard.Against.Null(errors, nameof(errors));

            foreach (var error in errors)
            {
                modelState.AddModelError(string.Empty, error);
            }
        }

        /// <summary>
        /// Shorthand method for ModelState.AddModelError(string.Empty, error);
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="error"></param>
        public static void AddModelError(this ModelStateDictionary modelState, string error)
        {
            Guard.Against.Null(modelState, nameof(modelState));

            modelState.AddModelError(string.Empty, error);
        }

        /// <summary>
        /// Used for cleaner code when multiple if cases are present
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="condition"></param>
        /// <param name="error"></param>
        public static void AddModelErrorIf(this ModelStateDictionary modelState, bool condition, string error)
        {
            Guard.Against.Null(modelState, nameof(modelState));

            if (condition)
            {
                modelState.AddModelError(string.Empty, error);
            }
        }

        /// <summary>
        /// Adds <see cref="Cleaners.Models.Result.Errors"/> to model state dictionary
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="errors"></param>
        public static void AddResults(this ModelStateDictionary modelState, IEnumerable<ResultError> errors)
        {
            Guard.Against.Null(modelState, nameof(modelState));
            Guard.Against.Null(errors, nameof(errors));

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Message);
            }
        }

        #endregion Methods
    }
}