using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Extensions;

internal static class TagHelperOutputExtensions
{
    /// <summary>
    /// Shorthand method for <see cref="TagHelperOutput.WriteTo(TextWriter, HtmlEncoder)"/>
    /// </summary>
    /// <param name="output">Tag helper output</param>
    /// <param name="htmlEncoder">Content encoder</param>
    /// <returns>Rendered content</returns>
    public static string Render(this TagHelperOutput output, HtmlEncoder htmlEncoder)
    {
        using var writer = new StringWriter();
        output.WriteTo(writer, htmlEncoder);
        return writer.ToString();
    }
}