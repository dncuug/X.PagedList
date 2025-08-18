using System.Collections.Generic;

namespace X.PagedList.Mvc.Core;

/// <summary>
/// Represents AJAX options for unobtrusive HTML attributes in MVC views.
/// Provides configuration for AJAX requests, including HTTP method, update target, event handlers, and caching.
/// </summary>
public class AjaxOptions
{
    /// <summary>
    /// Converts the current <see cref="AjaxOptions"/> instance into a collection of unobtrusive HTML attributes.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{HtmlAttribute}"/> containing the corresponding HTML attributes for AJAX functionality.
    /// </returns>
    public virtual IEnumerable<HtmlAttribute> ToUnobtrusiveHtmlAttributes()
    {
        var attrs = new List<HtmlAttribute>
        {
            new() { Key = "data-ajax-method", Value = HttpMethod },
            new() { Key = "data-ajax-mode", Value = InsertionMode },
            new() { Key = "data-ajax-update", Value = "#" + UpdateTargetId },
            new() { Key = "data-ajax", Value = "true" }
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
    /// The HTTP method to make the request with. The default value is "GET".
    /// </summary>
    public string HttpMethod { get; set; } = "GET";

    /// <summary>
    /// The mode used to handle the data received as response. The default value is "Replace".
    /// </summary>
    public InsertionMode InsertionMode { get; set; } = InsertionMode.Replace;

    /// <summary>
    /// The ID of the target element to update with the AJAX response.
    /// </summary>
    public string? UpdateTargetId { get; set; }

    /// <summary>
    /// The confirmation message to display before making the AJAX request.
    /// </summary>
    public string? Confirm { get; set; }

    /// <summary>
    /// The duration (in milliseconds) to display the loading element.
    /// </summary>
    public int LoadingElementDuration { get; set; }

    /// <summary>
    /// The ID of the element to display while the AJAX request is loading.
    /// </summary>
    public string? LoadingElementId { get; set; }

    /// <summary>
    /// JavaScript function to call before the AJAX request begins.
    /// </summary>
    public string? OnBegin { get; set; }

    /// <summary>
    /// JavaScript function to call when the AJAX request completes.
    /// </summary>
    public string? OnComplete { get; set; }

    /// <summary>
    /// JavaScript function to call if the AJAX request fails.
    /// </summary>
    public string? OnFailure { get; set; }

    /// <summary>
    /// JavaScript function to call if the AJAX request succeeds.
    /// </summary>
    public string? OnSuccess { get; set; }

    /// <summary>
    /// The URL to send the AJAX request to.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Indicates whether the AJAX response should be cached.
    /// </summary>
    public bool AllowCache { get; set; }
}