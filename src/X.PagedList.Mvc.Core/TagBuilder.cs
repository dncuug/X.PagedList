using System.Collections.Generic;
using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Core
{
    using System.IO;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Html;

    internal sealed class TagBuilder : ITagBuilder
    {
        private readonly Microsoft.AspNetCore.Mvc.Rendering.TagBuilder _tagBuilder;

        public TagBuilder(string tagName)
        {
            _tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder(tagName);
            Attributes = _tagBuilder.Attributes;
        }

        public IDictionary<string, string> Attributes { get; }

        public void AddCssClass(string value)
        {
            _tagBuilder
                .AddCssClass(value);
        }

        public void AppendHtml(string innerHtml)
        {
            _tagBuilder.InnerHtml
                .AppendHtml(innerHtml);
        }

        public void MergeAttribute(string key, string value)
        {
            _tagBuilder
                .MergeAttribute(key, value);
        }

        public void SetInnerText(string innerText)
        {
            _tagBuilder.InnerHtml
                .SetContent(innerText);
        }

        public string ToString(TagRenderMode renderMode, HtmlEncoder encoder = null)
        {
            encoder = HtmlEncoder.Create(new TextEncoderSettings());

            using (var writer = new StringWriter() as TextWriter)
            {
                _tagBuilder.WriteTo(writer, encoder);

                return writer.ToString();
            }
        }

        public override string ToString()
        {
            return _tagBuilder
                .ToString();
        }
    }
}
