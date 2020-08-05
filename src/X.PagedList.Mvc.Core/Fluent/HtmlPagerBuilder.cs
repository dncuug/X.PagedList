using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Core.Fluent
{
    using System;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    internal sealed class HtmlPagerBuilder : IHtmlPagerBuilder
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly IPagedList _pagedList;

        private Func<int, string> _generatePageUrl;
        private PagedListRenderOptions _options;
        private string _partialViewName;

        public HtmlPagerBuilder(IHtmlHelper htmlHelper, IPagedList pagedList)
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

        public IHtmlContent Classic()
        {
            this._options = PagedListRenderOptions.Classic;

            return Build();
        }

        public IHtmlContent ClassicPlusFirstAndLast()
        {
            this._options = PagedListRenderOptions.ClassicPlusFirstAndLast;

            return Build();
        }

        public IHtmlContent Minimal()
        {
            this._options = PagedListRenderOptions.Minimal;

            return Build();
        }

        public IHtmlContent MinimalWithPageCountText()
        {
            this._options = PagedListRenderOptions.MinimalWithPageCountText;

            return Build();
        }

        public IHtmlContent MinimalWithItemCountText()
        {
            this._options = PagedListRenderOptions.MinimalWithItemCountText;

            return Build();
        }

        public IHtmlContent PageNumbersOnly()
        {
            this._options = PagedListRenderOptions.PageNumbersOnly;

            return Build();
        }

        public IHtmlContent OnlyShowFivePagesAtATime()
        {
            this._options = PagedListRenderOptions.OnlyShowFivePagesAtATime;

            return Build();
        }

        public IHtmlPagerBuilder WithPartialView(string partialViewName)
        {
            this._partialViewName = partialViewName;

            return this;
        }

        public IHtmlContent Build(PagedListRenderOptions options)
        {
            this._options = options ?? this._options;

            return Build();
        }

        public IHtmlContent Build()
        {
            if (string.IsNullOrWhiteSpace(this._partialViewName))
            {
                return this._htmlHelper.PagedListPager(this._pagedList, this._generatePageUrl, this._options);
            }

            this._htmlHelper.ViewBag.Options = this._options;
            this._htmlHelper.ViewBag.GeneratePageUrl = this._generatePageUrl;

            return this._htmlHelper.Partial(this._partialViewName, this._pagedList);
        }
    }
}
