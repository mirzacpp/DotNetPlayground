using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Net;

namespace Corvo.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Enables framework to decide which action to invoke by specifie form value.
    /// This is useful in case we have multiple submit buttons that invokes method with same parameters
    /// </summary>
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// List of allowed button names
        /// </summary>
        private readonly string[] _allowedFormButtonNames;

        /// <summary>
        /// Determines if both, name and value should be validated
        /// </summary>
        private readonly bool _shouldValidateValue;

        public FormValueRequiredAttribute(params string[] allowedFormButtonNames)
            : this(true, allowedFormButtonNames)
        {
        }

        public FormValueRequiredAttribute(bool shouldValidateValue, params string[] allowedFormButtonNames)
        {
            _allowedFormButtonNames = allowedFormButtonNames;
            _shouldValidateValue = shouldValidateValue;
        }

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
            if (routeContext.HttpContext.Request.Method != WebRequestMethods.Http.Post)
            {
                return false;
            }

            foreach (var allowedButton in _allowedFormButtonNames)
            {
                if (_shouldValidateValue)
                {
                    // Returns value with specified key or empty string otherwise
                    var value = routeContext.HttpContext.Request.Form[allowedButton];

                    if (!string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
                else
                {
                    // Validate for name only
                    // Allow method invocation if form collection contains entry with specified key
                    if (routeContext.HttpContext.Request.Form.Any(v => v.Key.Equals(allowedButton, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}