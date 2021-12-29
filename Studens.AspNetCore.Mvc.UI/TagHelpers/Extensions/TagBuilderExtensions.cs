using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Extensions;

internal static class TagBuilderExtensions
{
    /// <summary>
    /// Shorthand method for <see cref="TagBuilder.WriteTo(TextWriter, HtmlEncoder)"/>
    /// </summary>
    /// <param name="tagBuilder">Tag builder</param>
    /// <returns>Rendered string</returns>
    public static string ToHtmlString(this TagBuilder tagBuilder) =>
        tagBuilder.ToHtmlString(HtmlEncoder.Default);

    /// <summary>
    /// Shorthand method for <see cref="TagBuilder.WriteTo(TextWriter, HtmlEncoder)"/>
    /// </summary>
    /// <param name="tagBuilder">Tag builder</param>
    /// <param name="htmlEncoder">Content encoder</param>
    /// <returns>Rendered string</returns>
    public static string ToHtmlString(this TagBuilder tagBuilder, HtmlEncoder htmlEncoder)
    {
        using var writer = new StringWriter();
        tagBuilder.WriteTo(writer, htmlEncoder);
        return writer.ToString();
    }
}