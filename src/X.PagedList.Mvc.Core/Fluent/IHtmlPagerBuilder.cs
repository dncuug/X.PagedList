using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace X.PagedList.Mvc.Core.Fluent;

/// <summary>
/// Fluent builder for configuring and rendering pager HTML for ASP.NET Core MVC.
/// </summary>
/// <remarks>
/// Chain configuration methods to control links, pagination behavior, and layout, then call one of the
/// render methods (e.g., <see cref="Build()"/>, <see cref="Classic()"/>) to produce <see cref="IHtmlContent"/>.
/// </remarks>
[PublicAPI]
public interface IHtmlPagerBuilder
{
    /// <summary>
    /// Sets the URL factory used to generate a link for a given page index.
    /// </summary>
    /// <param name="builder">
    /// A delegate receiving a 1-based page number and returning the target URL.
    /// Return <c>null</c> to suppress navigation for that page.
    /// </param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder Url(Func<int, string?> builder);

    /// <summary>
    /// Configures whether and when to render the link to the first page.
    /// </summary>
    /// <param name="displayMode">Display mode controlling visibility of the \`First\` link.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

    /// <summary>
    /// Configures whether and when to render the link to the last page.
    /// </summary>
    /// <param name="displayMode">Display mode controlling visibility of the \`Last\` link.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

    /// <summary>
    /// Configures whether and when to render the link to the previous page.
    /// </summary>
    /// <param name="displayMode">Display mode controlling visibility of the \`Previous\` link.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

    /// <summary>
    /// Configures whether and when to render the link to the next page.
    /// </summary>
    /// <param name="displayMode">Display mode controlling visibility of the \`Next\` link.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

    /// <summary>
    /// Toggles rendering of individual numbered page links.
    /// </summary>
    /// <param name="displayMode">True to show numbered page links; false to hide them.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true);

    /// <summary>
    /// Toggles rendering of the page count and current location text (e.g., \`Page 2 of 10\`).
    /// </summary>
    /// <param name="displayMode">True to show the page count/current location; false to hide it.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true);

    /// <summary>
    /// Toggles rendering of ellipses when not all page numbers are displayed.
    /// </summary>
    /// <param name="displayMode">True to show ellipses; false to omit them.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true);

    /// <summary>
    /// Sets the maximum number of numeric page links to display at once.
    /// </summary>
    /// <param name="pageNumbers">Maximum count of page number links to render.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">May be thrown by implementations if the value is less than 1.</exception>
    IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers);

    /// <summary>
    /// Specifies the partial view used to render the pager.
    /// </summary>
    /// <param name="partialViewName">The name or path of the partial view.</param>
    /// <returns>The current builder instance.</returns>
    IHtmlPagerBuilder WithPartialView(string partialViewName);

    /// <summary>
    /// Renders a classic pager with previous/next and numbered links using default options.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent Classic();

    /// <summary>
    /// Renders a classic pager including explicit first and last page links.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent ClassicPlusFirstAndLast();

    /// <summary>
    /// Renders a minimal pager (typically previous/next only).
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent Minimal();

    /// <summary>
    /// Renders a minimal pager with page count text.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent MinimalWithPageCountText();

    /// <summary>
    /// Renders a minimal pager with item count text.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent MinimalWithItemCountText();

    /// <summary>
    /// Renders only the numeric page links (no previous/next).
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent PageNumbersOnly();

    /// <summary>
    /// Renders a pager that shows up to five page numbers at a time.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent OnlyShowFivePagesAtATime();

    /// <summary>
    /// Builds the pager using the current fluent configuration.
    /// </summary>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent Build();

    /// <summary>
    /// Builds the pager using the provided render options.
    /// </summary>
    /// <param name="options">Explicit render options to apply. Implementations may override fluent settings.</param>
    /// <returns>Rendered pager as <see cref="IHtmlContent"/>.</returns>
    IHtmlContent Build(PagedListRenderOptions? options);
}