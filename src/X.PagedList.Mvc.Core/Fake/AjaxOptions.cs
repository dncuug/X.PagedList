using System.Collections.Generic;

namespace X.PagedList.Mvc.Core.Fake
{
    public class AjaxOptions
    {
        public IEnumerable<HtmlAttribute> ToUnobtrusiveHtmlAttributes()
        {
            var result = new List<HtmlAttribute>();

            result.Add(new HtmlAttribute() { Key = "data-ajax-method", Value = this.HttpMethod });
            result.Add(new HtmlAttribute() { Key = "data-ajax-mode", Value = this.InsertionMode });
            result.Add(new HtmlAttribute() { Key = "data-ajax-update", Value = "#" + this.UpdateTargetId });
            result.Add(new HtmlAttribute() { Key = "data-ajax", Value = "true" });

            return result;
        }

        public string HttpMethod { get; set; }
        public InsertionMode InsertionMode { get; set; }
        public string UpdateTargetId { get; set; }
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
