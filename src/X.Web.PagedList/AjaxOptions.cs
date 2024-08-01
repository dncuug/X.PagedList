using System.Collections.Generic;

namespace X.Web.PagedList;

public class AjaxOptions
{
    public virtual IEnumerable<HtmlAttribute> ToUnobtrusiveHtmlAttributes()
    {
        var attrs = new List<HtmlAttribute>
        {
            new() {Key = "data-ajax-method", Value = HttpMethod},
            new() {Key = "data-ajax-mode", Value = InsertionMode},
            new() {Key = "data-ajax-update", Value = "#" + UpdateTargetId},
            new() {Key = "data-ajax", Value = "true"}
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

    /// <summary>
    /// The HTTP method to make the request with. Default value is "GET".
    /// </summary>
    public string HttpMethod { get; set; } = "GET";

    /// <summary>
    /// The mode used to handle the data received as response. Default value is "Replace".
    /// </summary>
    public InsertionMode InsertionMode { get; set; } = InsertionMode.Replace;

    public string? UpdateTargetId { get; set; }
    public string? Confirm { get; set; }
    public int LoadingElementDuration { get; set; }
    public string? LoadingElementId { get; set; }
    public string? OnBegin { get; set; }
    public string? OnComplete { get; set; }
    public string? OnFailure { get; set; }
    public string? OnSuccess { get; set; }
    public string? Url { get; set; }
    public bool AllowCache { get; set; }
}

public enum InsertionMode
{
    Replace
}

/// <summary>
/// Represents one attribute of a DOM element
/// </summary>
/// <remarks>
/// Setting <see cref="Key"/> and <see cref="Value"/> is required.
/// </remarks>
public class HtmlAttribute
{
    public string Key { get; set; }
    public object Value { get; set; }
}
