using System.IO;
using System.Text.Encodings.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.Web.PagedList;

[PublicAPI]
public static class TagBuilderExtensions
{
    public static void AddCssClass(this TagBuilder tagBuilder, string value)
    {
        tagBuilder.AddCssClass(value);
    }

    public static void AppendHtml(this TagBuilder tagBuilder, string innerHtml)
    {
        tagBuilder.InnerHtml.AppendHtml(innerHtml);
    }

    public static void MergeAttribute(this TagBuilder tagBuilder, string key, string? value)
    {
        tagBuilder.MergeAttribute(key, value);
    }

    public static void SetInnerText(this TagBuilder tagBuilder, string innerText)
    {
        tagBuilder.InnerHtml.SetContent(innerText);
    }

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
