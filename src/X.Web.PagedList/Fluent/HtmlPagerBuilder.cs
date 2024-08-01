using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace X.Web.PagedList.Fluent;

internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
{
    private readonly IHtmlHelper _htmlHelper;
    private readonly IPagedList _pagedList;

    private Func<int, string?> _generatePageUrl;
    private PagedListRenderOptions _options;
    private string? _partialViewName;

    public HtmlPagerBuilder(IHtmlHelper htmlHelper, IPagedList pagedList)
    {
        _htmlHelper = htmlHelper;
        _pagedList = pagedList;
        _generatePageUrl = x => x.ToString();
        _options = new PagedListRenderOptions();
    }

    public IHtmlPagerBuilder Url(Func<int, string?> builder)
    {
        _generatePageUrl = builder;

        return this;
    }

    public IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToFirstPage = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToLastPage = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToPreviousPage = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
    {
        _options.DisplayLinkToNextPage = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true)
    {
        _options.DisplayLinkToIndividualPages = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true)
    {
        _options.DisplayPageCountAndCurrentLocation = displayMode;

        return this;
    }

    public IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true)
    {
        _options.DisplayEllipsesWhenNotShowingAllPageNumbers = displayMode;

        return this;
    }

    public IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers)
    {
        _options.MaximumPageNumbersToDisplay = pageNumbers;

        return this;
    }

    public IHtmlContent Classic()
    {
        _options = PagedListRenderOptions.Classic;

        return Build();
    }

    public IHtmlContent ClassicPlusFirstAndLast()
    {
        _options = PagedListRenderOptions.ClassicPlusFirstAndLast;

        return Build();
    }

    public IHtmlContent Minimal()
    {
        _options = PagedListRenderOptions.Minimal;

        return Build();
    }

    public IHtmlContent MinimalWithPageCountText()
    {
        _options = PagedListRenderOptions.MinimalWithPageCountText;

        return Build();
    }

    public IHtmlContent MinimalWithItemCountText()
    {
        _options = PagedListRenderOptions.MinimalWithItemCountText;

        return Build();
    }

    public IHtmlContent PageNumbersOnly()
    {
        _options = PagedListRenderOptions.PageNumbersOnly;

        return Build();
    }

    public IHtmlContent OnlyShowFivePagesAtATime()
    {
        _options = PagedListRenderOptions.OnlyShowFivePagesAtATime;

        return Build();
    }

    public IHtmlPagerBuilder WithPartialView(string partialViewName)
    {
        _partialViewName = partialViewName;

        return this;
    }

    public IHtmlContent Build(PagedListRenderOptions? options)
    {
        _options = options ?? _options;

        return Build();
    }

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
