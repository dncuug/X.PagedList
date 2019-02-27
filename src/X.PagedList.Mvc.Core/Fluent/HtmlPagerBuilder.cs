namespace X.PagedList.Mvc.Core.Fluent
{
    using System;
    using Common;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
    {
        private readonly IHtmlHelper htmlHelper;
        private readonly IPagedList pagedList;

        private Func<int, string> generatePageUrl;
        private PagedListRenderOptionsBase options;
        private string partialViewName;

        public HtmlPagerBuilder(IHtmlHelper htmlHelper, IPagedList pagedList)
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

        public IHtmlContent Classic()
        {
            this.options = PagedListRenderOptionsBase.Classic;

            return Build();
        }

        public IHtmlContent ClassicPlusFirstAndLast()
        {
            this.options = PagedListRenderOptionsBase.ClassicPlusFirstAndLast;

            return Build();
        }

        public IHtmlContent Minimal()
        {
            this.options = PagedListRenderOptionsBase.Minimal;

            return Build();
        }

        public IHtmlContent MinimalWithPageCountText()
        {
            this.options = PagedListRenderOptionsBase.MinimalWithPageCountText;

            return Build();
        }

        public IHtmlContent MinimalWithItemCountText()
        {
            this.options = PagedListRenderOptionsBase.MinimalWithItemCountText;

            return Build();
        }

        public IHtmlContent PageNumbersOnly()
        {
            this.options = PagedListRenderOptionsBase.PageNumbersOnly;

            return Build();
        }

        public IHtmlContent OnlyShowFivePagesAtATime()
        {
            this.options = PagedListRenderOptionsBase.OnlyShowFivePagesAtATime;

            return Build();
        }

        public IHtmlPagerBuilder WithPartialView(string partialViewName)
        {
            this.partialViewName = partialViewName;

            return this;
        }

        public IHtmlContent Build(PagedListRenderOptions options)
        {
            this.options = options ?? this.options;

            return Build();
        }

        public IHtmlContent Build()
        {
            if (string.IsNullOrWhiteSpace(this.partialViewName))
            {
                return this.htmlHelper.PagedListPager(this.pagedList, this.generatePageUrl, this.options);
            }

            this.htmlHelper.ViewBag.Options = this.options;
            this.htmlHelper.ViewBag.GeneratePageUrl = this.generatePageUrl;

            return this.htmlHelper.Partial(this.partialViewName, this.pagedList);
        }
    }
}
