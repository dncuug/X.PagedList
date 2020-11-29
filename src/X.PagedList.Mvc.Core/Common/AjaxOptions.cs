using System.Collections.Generic;

namespace X.PagedList.Web.Common
{
    public class AjaxOptions
    {
        public virtual IEnumerable<HtmlAttribute> ToUnobtrusiveHtmlAttributes()
        {
            var attrs = new List<HtmlAttribute>
            {
                new HtmlAttribute {Key = "data-ajax-method", Value = HttpMethod},
                new HtmlAttribute {Key = "data-ajax-mode", Value = InsertionMode},
                new HtmlAttribute {Key = "data-ajax-update", Value = "#" + UpdateTargetId},
                new HtmlAttribute {Key = "data-ajax", Value = "true"}
            };

            if (!string.IsNullOrEmpty(Confirm))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-confirm", Value = Confirm });
            }

            if (!string.IsNullOrEmpty(LoadingElementId))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-loading", Value = LoadingElementId });
            }

            if (LoadingElementDuration > 0)
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-loading-duration", Value = LoadingElementDuration });
            }

            if (!string.IsNullOrEmpty(OnBegin))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-begin", Value = OnBegin });
            }

            if (!string.IsNullOrEmpty(OnComplete))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-complete", Value = OnComplete });
            }

            if (!string.IsNullOrEmpty(OnFailure))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-failure", Value = OnFailure });
            }

            if (!string.IsNullOrEmpty(OnSuccess))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-success", Value = OnSuccess });
            }

            if (!string.IsNullOrEmpty(Url))
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-url", Value = Url });
            }

            if (AllowCache)
            {
                attrs.Add(new HtmlAttribute { Key = "data-ajax-cache", Value = "true" });
            }

            return attrs;
        }

        public string HttpMethod { get; set; }
        public InsertionMode InsertionMode { get; set; }
        public string UpdateTargetId { get; set; }
        public string Confirm { get; set; }
        public int LoadingElementDuration { get; set; }
        public string LoadingElementId { get; set; }
        public string OnBegin { get; set; }
        public string OnComplete { get; set; }
        public string OnFailure { get; set; }
        public string OnSuccess { get; set; }
        public string Url { get; set; }
        public bool AllowCache { get; set; }
    }

    public enum InsertionMode
    {
        Replace
    }

    public class HtmlAttribute
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}