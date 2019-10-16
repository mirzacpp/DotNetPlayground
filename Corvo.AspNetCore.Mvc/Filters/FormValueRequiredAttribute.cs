using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace Corvo.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Enables framework to decide which action to invoke by specifie form value.
    /// This is useful in case we have multiple submit buttons that invokes method with same parameters
    /// </summary>
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        #region Fields

        /// <summary>
        /// List of allowed form data element names
        /// </summary>
        private readonly string[] _allowedFormDataElementNames;

        /// <summary>
        /// Determines if both, name and value should be validated
        /// </summary>
        private readonly bool _shouldValidateValue;

        #endregion Fields

        #region Constructors

        public FormValueRequiredAttribute(params string[] allowedFormButtonNames)
        : this(true, allowedFormButtonNames) { }

        public FormValueRequiredAttribute(bool shouldValidateValue, params string[] allowedFormDataElementNames)
        {
            _allowedFormDataElementNames = allowedFormDataElementNames;
            _shouldValidateValue = shouldValidateValue;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Verifies if request is allowed to invoke specified action
        /// </summary>
        /// <param name="routeContext"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (routeContext == null)
            {
                throw new ArgumentNullException(nameof(routeContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Only POST requests should be verified
            if (!HttpMethods.IsPost(routeContext.HttpContext.Request.Method))
            {
                return false;
            }

            foreach (var allowedElement in _allowedFormDataElementNames)
            {
                if (_shouldValidateValue)
                {
                    // Returns value with specified key or empty string otherwise
                    var value = routeContext.HttpContext.Request.Form[allowedElement];

                    if (!string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
                else
                {
                    // Validate for name only
                    // Allow method invocation if form collection contains entry with specified key
                    if (routeContext.HttpContext.Request.Form.Any(v => v.Key.Equals(allowedElement, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Methods
    }
}