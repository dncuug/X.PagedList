namespace X.PagedList.Mvc.Fluent
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Common;

    internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
    {
        private readonly HtmlHelper htmlHelper;
        private readonly IPagedList pagedList;

        private Func<int, string> generatePageUrl;
        private PagedListRenderOptionsBase options;
        private string partialViewName;

        public HtmlPagerBuilder(HtmlHelper htmlHelper, IPagedList pagedList)
        {
            this.htmlHelper = htmlHelper;
            this.pagedList = pagedList;
            this.generatePageUrl = x => x.ToString();
            this.options = new PagedListRenderOptions();
        }

        public IHtmlPagerBuilder Url(Func<int, string> builder)
        {
            this.generatePageUrl = builder;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToFirstPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this.options.DisplayLinkToFirstPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToLastPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this.options.DisplayLinkToLastPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToPreviousPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this.options.DisplayLinkToPreviousPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToNextPage(PagedListDisplayMode displayMode = PagedListDisplayMode.Always)
        {
            this.options.DisplayLinkToNextPage = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayLinkToIndividualPages(bool displayMode = true)
        {
            this.options.DisplayLinkToIndividualPages = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayPageCountAndCurrentLocation(bool displayMode = true)
        {
            this.options.DisplayPageCountAndCurrentLocation = displayMode;

            return this;
        }

        public IHtmlPagerBuilder DisplayEllipsesWhenNotShowingAllPageNumbers(bool displayMode = true)
        {
            this.options.DisplayEllipsesWhenNotShowingAllPageNumbers = displayMode;

            return this;
        }

        public IHtmlPagerBuilder MaximumPageNumbersToDisplay(int pageNumbers)
        {
            this.options.MaximumPageNumbersToDisplay = pageNumbers;

            return this;
        }

        public IHtmlString Classic()
        {
            this.options = PagedListRenderOptionsBase.Classic;

            return Build();
        }

        public IHtmlString ClassicPlusFirstAndLast()
        {
            this.options = PagedListRenderOptionsBase.ClassicPlusFirstAndLast;

            return Build();
        }

        public IHtmlString Minimal()
        {
            this.options = PagedListRenderOptionsBase.Minimal;

            return Build();
        }

        public IHtmlString MinimalWithPageCountText()
        {
            this.options = PagedListRenderOptionsBase.MinimalWithPageCountText;

            return Build();
        }

        public IHtmlString MinimalWithItemCountText()
        {
            this.options = PagedListRenderOptionsBase.MinimalWithItemCountText;

            return Build();
        }

        public IHtmlString PageNumbersOnly()
        {
            this.options = PagedListRenderOptionsBase.PageNumbersOnly;

            return Build();
        }

        public IHtmlString OnlyShowFivePagesAtATime()
        {
            this.options = PagedListRenderOptionsBase.OnlyShowFivePagesAtATime;

            return Build();
        }

        public IHtmlPagerBuilder WithPartialView(string partialViewName)
        {
            this.partialViewName = partialViewName;

            return this;
        }

        public IHtmlString Build(PagedListRenderOptions options)
        {
            this.options = options ?? this.options;

            return Build();
        }

        public IHtmlString Build()
        {
            if (string.IsNullOrWhiteSpace(this.partialViewName))
            {
                return this.htmlHelper.PagedListPager(this.pagedList, this.generatePageUrl, this.options);
            }

            this.htmlHelper.ViewData["Options"] = this.options;
            this.htmlHelper.ViewData["GeneratePageUrl"] = this.generatePageUrl;

            return this.htmlHelper.Partial(this.partialViewName, this.pagedList);
        }
    }
}
