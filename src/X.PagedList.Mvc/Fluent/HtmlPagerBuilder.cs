using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Fluent
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly IPagedList _pagedList;

        private Func<int, string> _generatePageUrl;
        private PagedListRenderOptions _options;
        private string _partialViewName;

        public HtmlPagerBuilder(HtmlHelper htmlHelper, IPagedList pagedList)
        {
            this._htmlHelper = htmlHelper;
            this._pagedList = pagedList;
            this._generatePageUrl = x => x.ToString();
            this._options = new PagedListRenderOptions();
        }

        public IHtmlPagerBuilder Url(Func<int, string> builder)
        {
            this._generatePageUrl = builder;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this._options.DisplayLinkToFirstPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this._options.DisplayLinkToLastPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this._options.DisplayLinkToPreviousPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this._options.DisplayLinkToNextPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true)
        {
            this._options.DisplayLinkToIndividualPages = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true)
        {
            this._options.DisplayPageCountAndCurrentLocation = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true)
        {
            this._options.DisplayEllipsesWhenNotShowingAllPageNumbers = displayMode;

            return this;
        }

        public IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers)
        {
            this._options.MaximumPageNumbersToDisplay = pageNumbers;

            return this;
        }

        public IHtmlString Classic()
        {
            this._options = PagedListRenderOptions.Classic;

            return Build();
        }

        public IHtmlString ClassicPlusFirstAndLast()
        {
            this._options = PagedListRenderOptions.ClassicPlusFirstAndLast;

            return Build();
        }

        public IHtmlString Minimal()
        {
            this._options = PagedListRenderOptions.Minimal;

            return Build();
        }

        public IHtmlString MinimalWithPageCountText()
        {
            this._options = PagedListRenderOptions.MinimalWithPageCountText;

            return Build();
        }

        public IHtmlString MinimalWithItemCountText()
        {
            this._options = PagedListRenderOptions.MinimalWithItemCountText;

            return Build();
        }

        public IHtmlString PageNumbersOnly()
        {
            this._options = PagedListRenderOptions.PageNumbersOnly;

            return Build();
        }

        public IHtmlString OnlyShowFivePagesAtATime()
        {
            this._options = PagedListRenderOptions.OnlyShowFivePagesAtATime;

            return Build();
        }

        public IHtmlPagerBuilder WithPartialView(string partialViewName)
        {
            this._partialViewName = partialViewName;

            return this;
        }

        public IHtmlString Build(PagedListRenderOptions options)
        {
            this._options = options ?? this._options;

            return Build();
        }

        public IHtmlString Build()
        {
            if (string.IsNullOrWhiteSpace(this._partialViewName))
            {
                return this._htmlHelper.PagedListPager(this._pagedList, this._generatePageUrl, this._options);
            }

            this._htmlHelper.ViewData["Options"] = this._options;
            this._htmlHelper.ViewData["GeneratePageUrl"] = this._generatePageUrl;

            return this._htmlHelper.Partial(this._partialViewName, this._pagedList);
        }
    }
}
