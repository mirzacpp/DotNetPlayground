using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Extensions;

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
        attributes.AddIf(condition, new TagHelperAttribute(name, value));
    }

    public static void AddIf(this TagHelperAttributeList attributes, bool condition, TagHelperAttribute attribute)
    {
        Guard.Against.Null(attributes, nameof(attributes));

        if (condition)
        {
            attributes.Add(attribute);
        }
    }

    /// <summary>
    /// Appends given <paramref name="className"/> to <paramref name="attributes"/>
    /// </summary>
    public static void AddClass(this TagHelperAttributeList attributes, string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        var classes = attributes["class"];

        if (classes is null)
        {
            attributes.Add("class", className);
        }
        else
        {
            var existingClasses = classes.Value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            existingClasses.AddIfNotContains(className);
            attributes.SetAttribute("class", string.Join(" ", existingClasses));
        }
    }
}