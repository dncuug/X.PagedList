using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Fluent
{
    using System;
    using System.Web;

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

        IHtmlString Classic();

        IHtmlString ClassicPlusFirstAndLast();

        IHtmlString Minimal();

        IHtmlString MinimalWithPageCountText();

        IHtmlString MinimalWithItemCountText();

        IHtmlString PageNumbersOnly();

        IHtmlString OnlyShowFivePagesAtATime();

        IHtmlString Build();

        IHtmlString Build(PagedListRenderOptions options);
    }
}
