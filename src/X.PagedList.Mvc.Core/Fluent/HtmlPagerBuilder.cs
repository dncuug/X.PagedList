using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.PagedList.Mvc.Core.Fluent;

/// <summary>
/// Fluent builder that configures and renders a pager for an <see cref="IPagedList"/> in ASP.NET Core MVC.
/// </summary>
/// <remarks>
/// Uses <see cref="PagedListRenderOptions"/> to configure output and <see cref="IHtmlHelper"/> to render either
/// the default pager or a custom partial view.
/// </remarks>
internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
{
    private readonly IHtmlHelper _htmlHelper;
    private readonly IPagedList _pagedList;

    private Func<int, string?> _generatePageUrl;
    private PagedListRenderOptions _options;
    private string? _partialViewName;

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlPagerBuilder"/> class.
    /// </summary>
    /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> used to render HTML.</param>
    /// <param name="pagedList">The source <see cref="IPagedList"/> to paginate.</param>
    public HtmlPagerBuilder(IHtmlHelper htmlHelper, IPagedList pagedList)
    {
        _htmlHelper = htmlHelper;
        _pagedList = pagedList;
        _generatePageUrl = x => x.ToString();
        _options = new PagedListRenderOptions();
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder Url(Func<int, string?> builder)
    {
        _generatePageUrl = builder;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToFirstPage = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToLastPage = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToPreviousPage = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToNextPage = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true)
    {
        _options.DisplayLinkToIndividualPages = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true)
    {
        _options.DisplayPageCountAndCurrentLocation = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true)
    {
        _options.DisplayEllipsesWhenNotShowingAllPageNumbers = displayMode;

        return this;
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers)
    {
        _options.MaximumPageNumbersToDisplay = pageNumbers;

        return this;
    }

    /// <inheritdoc />
    public IHtmlContent Classic()
    {
        _options = PagedListRenderOptions.Classic;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent ClassicPlusFirstAndLast()
    {
        _options = PagedListRenderOptions.ClassicPlusFirstAndLast;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent Minimal()
    {
        _options = PagedListRenderOptions.Minimal;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent MinimalWithPageCountText()
    {
        _options = PagedListRenderOptions.MinimalWithPageCountText;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent MinimalWithItemCountText()
    {
        _options = PagedListRenderOptions.MinimalWithItemCountText;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent PageNumbersOnly()
    {
        _options = PagedListRenderOptions.PageNumbersOnly;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent OnlyShowFivePagesAtATime()
    {
        _options = PagedListRenderOptions.OnlyShowFivePagesAtATime;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlPagerBuilder WithPartialView(string partialViewName)
    {
        _partialViewName = partialViewName;

        return this;
    }

    /// <inheritdoc />
    public IHtmlContent Build(PagedListRenderOptions? options)
    {
        _options = options ?? _options;

        return Build();
    }

    /// <inheritdoc />
    public IHtmlContent Build()
    {
        if (string.IsNullOrWhiteSpace(_partialViewName))
        {
            return _htmlHelper.PagedListPager(_pagedList, _generatePageUrl, _options);
        }

        _htmlHelper.ViewBag.Options = _options;
        _htmlHelper.ViewBag.GeneratePageUrl = _generatePageUrl;

        return _htmlHelper.Partial(_partialViewName, _pagedList);
    }
}