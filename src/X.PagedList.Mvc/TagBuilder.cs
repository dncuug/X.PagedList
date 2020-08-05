using System.Collections.Generic;
using X.PagedList.Web.Common;

namespace X.PagedList.Mvc
{
    using System.Text.Encodings.Web;

    internal sealed class TagBuilder : ITagBuilder
    {
        private readonly System.Web.Mvc.TagBuilder _tagBuilder;

        public TagBuilder(string tagName)
        {
            _tagBuilder = new System.Web.Mvc.TagBuilder(tagName);
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
            _tagBuilder.InnerHtml += innerHtml;
        }

        public void MergeAttribute(string key, string value)
        {
            _tagBuilder
                .MergeAttribute(key, value);
        }

        public void SetInnerText(string innerText)
        {
            _tagBuilder
                .SetInnerText(innerText);
        }

        public string ToString(TagRenderMode renderMode, HtmlEncoder encoder = null)
        {
            return _tagBuilder
                .ToString((System.Web.Mvc.TagRenderMode)(int)renderMode);
        }

        public override string ToString()
        {
            return _tagBuilder
                .ToString();
        }
    }
}
