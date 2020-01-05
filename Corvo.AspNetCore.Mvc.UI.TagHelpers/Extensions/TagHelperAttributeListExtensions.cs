using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Corvo.AspNetCore.Mvc.UI.TagHelpers.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TagHelperAttributeList"/> collection
    /// </summary>
    public static class TagHelperAttributeListExtensions
    {
        /// <summary>
        /// Shorthand method to add attribute to collection if condition is satisfied
        /// </summary>
        public static void AddIf(this TagHelperAttributeList attributes, bool condition, string name, object value)
        {
            // Framework internaly invokes Add overload with TagHelperAttribute argument
            AddIf(attributes, condition, new TagHelperAttribute(name, value));
        }

        public static void AddIf(this TagHelperAttributeList attributes, bool condition, TagHelperAttribute attribute)
        {
            Guard.Against.Null(attributes, nameof(attributes));

            if (condition)
            {
                attributes.Add(attribute);
            }
        }
    }
}