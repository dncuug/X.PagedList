using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace X.PagedList.Mvc.Core.Fluent;

/// <summary>
/// Extension methods for generating pager markup for <c>X.PagedList</c> in ASP.NET Core MVC.
/// </summary>
/// <remarks>
/// Provides a simple entry point to either build and render a pager immediately, or obtain a fluent builder
/// (<see cref="IHtmlPagerBuilder"/>) to further configure the pager before rendering.
/// </remarks>
[PublicAPI]
public static class HtmlPagerExtensions
{
    /// <summary>
    /// Builds and returns an HTML pager for an empty paged list.
    /// </summary>
    /// <param name="htmlHelper">The HTML helper used to render the pager.</param>
    /// <returns>An <see cref="IHtmlContent"/> containing the pager markup.</returns>
    /// <remarks>
    /// Useful when the current view model is not an <see cref="IPagedList"/> but a pager still needs to be rendered.
    /// Internally constructs an empty paged list via <see cref="Enumerable.Empty{TResult}"/> and <c>ToPagedList()</c>.
    /// </remarks>
    public static IHtmlContent Pager(this IHtmlHelper htmlHelper)
    {
        return new HtmlPagerBuilder(htmlHelper, Enumerable.Empty<object?>().ToPagedList()).Build();
    }

    /// <summary>
    /// Creates a fluent pager builder for the specified paged list.
    /// </summary>
    /// <param name="htmlHelper">The HTML helper used to configure and render the pager.</param>
    /// <param name="list">The paged list providing paging metadata (page number, size, total item count).</param>
    /// <returns>An <see cref="IHtmlPagerBuilder"/> that can be further configured and rendered.</returns>
    public static IHtmlPagerBuilder Pager(this IHtmlHelper htmlHelper, IPagedList list)
    {
        return new HtmlPagerBuilder(htmlHelper, list);
    }
}