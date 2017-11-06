using System.Collections.Generic;

namespace X.PagedList.Mvc.Core.Fake
{
    public class AjaxOptions
    {
        public IEnumerable<HtmlAttribute> ToUnobtrusiveHtmlAttributes()
        {
            return new List<HtmlAttribute>
            {
                new HtmlAttribute {Key = "data-ajax-method", Value = this.HttpMethod},
                new HtmlAttribute {Key = "data-ajax-mode", Value = this.InsertionMode},
                new HtmlAttribute {Key = "data-ajax-update", Value = "#" + this.UpdateTargetId},
                new HtmlAttribute {Key = "data-ajax", Value = "true"}
            };
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
