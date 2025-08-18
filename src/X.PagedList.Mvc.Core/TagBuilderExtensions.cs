using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.PagedList.Mvc.Core;

/// <summary>
/// Extension methods for <see cref="TagBuilder"/> to simplify common operations
/// like adding CSS classes, merging attributes, manipulating inner content,
/// and rendering the tag to a string with a configurable <see cref="HtmlEncoder"/>.
/// </summary>
public static class TagBuilderExtensions
{
    /// <summary>
    /// Adds one or more CSS classes to the underlying HTML element.
    /// </summary>
    /// <param name="tagBuilder">The target <see cref="TagBuilder"/>.</param>
    /// <param name="value">A space-separated list of CSS class names to add.</param>
    public static void AddCssClass(this TagBuilder tagBuilder, string value)
    {
        tagBuilder.AddCssClass(value);
    }

    /// <summary>
    /// Appends raw HTML to the element's inner HTML without encoding.
    /// </summary>
    /// <param name="tagBuilder">The target <see cref="TagBuilder"/>.</param>
    /// <param name="innerHtml">The HTML fragment to append.</param>
    public static void AppendHtml(this TagBuilder tagBuilder, string innerHtml)
    {
        tagBuilder.InnerHtml.AppendHtml(innerHtml);
    }

    /// <summary>
    /// Adds a new attribute or updates an existing one on the element.
    /// </summary>
    /// <param name="tagBuilder">The target <see cref="TagBuilder"/>.</param>
    /// <param name="key">The attribute name.</param>
    /// <param name="value">The attribute value. May be <c>null</c>.</param>
    public static void MergeAttribute(this TagBuilder tagBuilder, string key, string? value)
    {
        tagBuilder.MergeAttribute(key, value);
    }

    /// <summary>
    /// Sets the inner text content of the element, HTML-encoding the provided text.
    /// </summary>
    /// <param name="tagBuilder">The target <see cref="TagBuilder"/>.</param>
    /// <param name="innerText">The text to set as the element's content.</param>
    public static void SetInnerText(this TagBuilder tagBuilder, string innerText)
    {
        tagBuilder.InnerHtml.SetContent(innerText);
    }

    /// <summary>
    /// Renders the <see cref="TagBuilder"/> to a string using the specified render mode and encoder.
    /// </summary>
    /// <param name="tagBuilder">The target <see cref="TagBuilder"/>.</param>
    /// <param name="renderMode">The <see cref="TagRenderMode"/> to use when rendering.</param>
    /// <param name="encoder">
    /// Optional <see cref="HtmlEncoder"/> to use during rendering. If not provided, a default encoder is created.
    /// </param>
    /// <returns>The rendered HTML string.</returns>
    public static string ToString(this TagBuilder tagBuilder, TagRenderMode renderMode, HtmlEncoder? encoder = null)
    {
        encoder ??= HtmlEncoder.Create(new TextEncoderSettings());

        using (var writer = new StringWriter())
        {
            tagBuilder.WriteTo(writer, encoder);

            return writer.ToString();
        }
    }
}