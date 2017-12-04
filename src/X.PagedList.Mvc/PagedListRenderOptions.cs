using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using X.PagedList.Mvc.Common;

namespace X.PagedList.Mvc
{
    ///<summary>
    /// Options for configuring the output of <see cref = "X.PagedList.Mvc.HtmlHelper" />
    ///</summary>
    public class PagedListRenderOptions : PagedListRenderOptionsBase
    {
        /// <summary>
        /// An extension point which allows you to fully customize the anchor tags used for clickable pages, as well as navigation features such as Next, Last, etc.
        /// </summary>
        public Func<TagBuilder, TagBuilder, TagBuilder> FunctionToTransformEachPageLink { get; set; }

        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <param name="options">The preferred Html.PagedList(...) style options.</param>
        /// <param name="ajaxOptions">The ajax options that will put into the link</param>
        /// <returns>The PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        public static PagedListRenderOptionsBase EnableUnobtrusiveAjaxReplacing(PagedListRenderOptionsBase options, AjaxOptions ajaxOptions)
        {
            if (options is PagedListRenderOptions)
            {
                ((PagedListRenderOptions) options).FunctionToTransformEachPageLink = (liTagBuilder, aTagBuilder) =>
                {
                    var liClass = liTagBuilder.Attributes.ContainsKey("class")
                        ? liTagBuilder.Attributes["class"] ?? ""
                        : "";
                    if (ajaxOptions != null && !liClass.Contains("disabled") && !liClass.Contains("active"))
                    {
                        foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                            aTagBuilder.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
                    }

                    liTagBuilder.InnerHtml = aTagBuilder.ToString();
                    return liTagBuilder;
                };
            }

            return options;
        }

        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <param name="id">The element ID ("my_id") of the element whose innerHtml should be replaced, if # is included at the start this will be removed.</param>
        /// <returns>A default instance of PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        public static PagedListRenderOptionsBase EnableUnobtrusiveAjaxReplacing(string id)
        {

            if (id.StartsWith("#"))
                id = id.Substring(1);

            var ajaxOptions = new AjaxOptions()
            {
                HttpMethod = "GET",
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = id
            };

            return EnableUnobtrusiveAjaxReplacing(new PagedListRenderOptions(), ajaxOptions);
        }

        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <param name="ajaxOptions">Ajax options that will be used to generate the unobstrusive tags on the link</param>
        /// <returns>A default instance of PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        public static PagedListRenderOptionsBase EnableUnobtrusiveAjaxReplacing(AjaxOptions ajaxOptions)
        {
            return EnableUnobtrusiveAjaxReplacing(new PagedListRenderOptions(), ajaxOptions);
        }

    }
}