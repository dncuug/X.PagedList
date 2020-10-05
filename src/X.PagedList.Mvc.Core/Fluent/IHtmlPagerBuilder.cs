using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Core.Fluent
{
    using System;
    using Microsoft.AspNetCore.Html;

    public interface IHtmlPagerBuilder
    {
        IHtmlPagerBuilder Url(Func<int, string> builder);

        IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

        IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

        IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

        IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always);

        IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true);

        IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true);

        IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true);

        IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers);

        IHtmlPagerBuilder WithPartialView(string partialViewName);

        IHtmlContent Classic();

        IHtmlContent ClassicPlusFirstAndLast();

        IHtmlContent Minimal();

        IHtmlContent MinimalWithPageCountText();

        IHtmlContent MinimalWithItemCountText();

        IHtmlContent PageNumbersOnly();

        IHtmlContent OnlyShowFivePagesAtATime();

        IHtmlContent Build();

        IHtmlContent Build(PagedListRenderOptions options);
    }
}
