using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Web.Mvc.Ajax; // Added into project

namespace X.PagedList.Core.TagHelpers
{
    ///<summary>
    ///	Displays a configurable paging control for instances of PagedList.
    ///</summary>
    public class PagedListPagerTagHelper : TagHelper
    {
        #region Input parameters
        ///<summary>
        /// The PagedList to use as the data source.
        ///</summary>
        public IPagedList List { get; set; }

        ///<summary>
        /// A function that takes the page number  of the desired page and returns a URL-string that will load that page.
        ///</summary>
        public Func<int, string> PageUrl { get; set; }

        ///<summary>
        /// CSS Classes to append to the &lt;div&gt; element that wraps the paging control.
        ///</summary>
        public IEnumerable<string> ContainerDivClasses { get; set; }

        ///<summary>
        /// CSSClasses to append to the &lt;ul&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> UlElementClasses { get; set; }

        /// <summary>
        /// Attrinutes to appendto the &lt;ul&gt; element in the paging control
        /// </summary>
        public IDictionary<string, string> UlElementattributes { get; set; }

        ///<summary>
        /// CSS Classes to append to every &lt;li&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> LiElementClasses { get; set; }

        /// <summary>
        /// CSS Classes to appent to active &lt;li&gt; element in the paging control.
        /// </summary>
        public string ActiveLiElementClass { get; set; }

        ///<summary>
        /// CSS Classes to append to every &lt;a&gt; or &lt;span&gt; element that represent each page in the paging control.
        ///</summary>
        public IEnumerable<string> PageClasses { get; set; }

        ///<summary>
        /// CSS Classes to append to previous element in the paging control.
        ///</summary>
        public string PreviousElementClass { get; set; }

        ///<summary>
        /// CSS Classes to append to next element in the paging control.
        ///</summary>
        public string NextElementClass { get; set; }

        ///<summary>
        /// CSS Classes to append to Ellipses element in the paging control.
        ///</summary>
        public string EllipsesElementClass { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the first list item in the pager. If null or whitespace is defined, no additional class is added to first list item in list.
        ///</summary>
        public string ClassToApplyToFirstListItemInPager { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the last list item in the pager. If null or whitespace is defined, no additional class is added to last list item in list.
        ///</summary>
        public string ClassToApplyToLastListItemInPager { get; set; }

        /// <summary>
        /// If set to Always, always renders the paging control. If set to IfNeeded, render the paging control when there is more than one page.
        /// </summary>
        public PagedListDisplayMode Display { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the first page in the list. If set to IfNeeded, render the hyperlink only when the first page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToFirstPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the last page in the list. If set to IfNeeded, render the hyperlink only when the last page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToLastPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the previous page of the list. If set to IfNeeded, render the hyperlink only when there is a previous page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToPreviousPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the next page of the list. If set to IfNeeded, render the hyperlink only when there is a next page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToNextPage { get; set; }

        ///<summary>
        /// When true, includes hyperlinks for each page in the list.
        ///</summary>
        public bool DisplayLinkToIndividualPages { get; set; }

        ///<summary>
        /// When true, shows the current page number and the total number of pages in the list.
        ///</summary>
        ///<example>
        /// "Page 3 of 8."
        ///</example>
        public bool DisplayPageCountAndCurrentLocation { get; set; }

        ///<summary>
        /// When true, shows the one-based index of the first and last items on the page, and the total number of items in the list.
        ///</summary>
        ///<example>
        /// "Showing items 75 through 100 of 183."
        ///</example>
        public bool DisplayItemSliceAndTotal { get; set; }

        ///<summary>
        /// The maximum number of page numbers to display. Null displays all page numbers.
        ///</summary>
        public int? MaximumPageNumbersToDisplay { get; set; }

        ///<summary>
        /// If true, adds an ellipsis where not all page numbers are being displayed.
        ///</summary>
        ///<example>
        /// "1 2 3 4 5 ...",
        /// "... 6 7 8 9 10 ...",
        /// "... 11 12 13 14 15"
        ///</example>
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

        ///<summary>
        /// The pre-formatted text to display when not all page numbers are displayed at once.
        ///</summary>
        ///<example>
        /// "..."
        ///</example>
        public string EllipsesFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the first page. The one-based index of the page (always 1 in this case) is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt;&lt; First"
        ///</example>
        public string LinkToFirstPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the previous page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt; Previous"
        ///</example>
        public string LinkToPreviousPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to each individual page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "{0}"
        ///</example>
        public string LinkToIndividualPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the next page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Next &gt;"
        ///</example>
        public string LinkToNextPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the last page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Last &gt;&gt;"
        ///</example>
        public string LinkToLastPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayPageCountAndCurrentLocation is true. Use {0} to reference the current page and {1} to reference the total number of pages.
        ///</summary>
        ///<example>
        /// "Page {0} of {1}."
        ///</example>
        public string PageCountAndCurrentLocationFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayItemSliceAndTotal is true. Use {0} to reference the first item on the page, {1} for the last item on the page, and {2} for the total number of items across all pages.
        ///</summary>
        ///<example>
        /// "Showing items {0} through {1} of {2}."
        ///</example>
        public string ItemSliceAndTotalFormat { get; set; }

        /// <summary>
        /// A function that will render each page number when specified (and DisplayLinkToIndividualPages is true). If no function is specified, the LinkToIndividualPageFormat value will be used instead.
        /// </summary>
        public Func<int, string> FunctionToDisplayEachPageNumber { get; set; }

        /// <summary>
        /// Text that will appear between each page number. If null or whitespace is specified, no delimiter will be shown.
        /// </summary>
        public string DelimiterBetweenPageNumbers { get; set; }

        /// <summary>
        /// An extension point which allows you to fully customize the anchor tags used for clickable pages, as well as navigation features such as Next, Last, etc.
        /// </summary>
        public Func<TagBuilder, TagBuilder, TagBuilder> FunctionToTransformEachPageLink { get; set; }

        #region Unobtrusive AJAX support
        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <returns>The PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        public AjaxOptions EnableUnobtrusiveAjaxReplacing
        {
            get { return new AjaxOptions(); }
            set
            {
                FunctionToTransformEachPageLink = (liTagBuilder, aTagBuilder) =>
                {
                    var liClass = liTagBuilder.Attributes.ContainsKey("class") ? liTagBuilder.Attributes["class"] ?? "" : "";
                    if (value != null && !liClass.Contains("disabled") && !liClass.Contains("active"))
                    {
                        foreach (var ajaxOption in value.ToUnobtrusiveHtmlAttributes())
                            aTagBuilder.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
                    }

                    HtmlHelper.AppendHtml(liTagBuilder, HtmlHelper.TagBuilderToString(aTagBuilder));
                    return liTagBuilder;
                };
            }
        }
        #endregion

        #endregion

        #region Predefined options
        ///<summary>
        /// Also includes links to First and Last pages.
        ///</summary>
        public bool PredefinedClassic
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
            }
        }

        ///<summary>
        /// Also includes links to First and Last pages.
        ///</summary>
        public bool PredefinedClassicPlusFirstAndLast
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always;
                DisplayLinkToLastPage = PagedListDisplayMode.Always;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
            }
        }

        ///<summary>
        /// Shows only the Previous and Next links.
        ///</summary>
        public bool PredefinedMinimal
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                DisplayLinkToIndividualPages = false;
            }
        }

        ///<summary>
        /// Shows Previous and Next links along with current page number and page count.
        ///</summary>
        public bool PredefinedMinimalWithPageCountText
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                DisplayLinkToIndividualPages = false;
                DisplayPageCountAndCurrentLocation = true;
            }
        }

        ///<summary>
        ///	Shows Previous and Next links along with index of first and last items on page and total number of items across all pages.
        ///</summary>
        public bool PredefinedMinimalWithItemCountText
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                DisplayLinkToIndividualPages = false;
                DisplayItemSliceAndTotal = true;
            }
        }

        ///<summary>
        ///	Shows only links to each individual page.
        ///</summary>
        public bool PredefinedPageNumbersOnly
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Never;
                DisplayLinkToNextPage = PagedListDisplayMode.Never;
                DisplayEllipsesWhenNotShowingAllPageNumbers = false;
            }
        }

        ///<summary>
        ///	Shows Next and Previous while limiting to a max of 5 page numbers at a time.
        ///</summary>
        public bool PredefinedOnlyShowFivePagesAtATime
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                MaximumPageNumbersToDisplay = 5;
            }
        }

        ///<summary>
        /// Twitter Bootstrap 2's basic pager format (just Previous and Next links).
        ///</summary>
        public bool PredefinedTwitterBootstrapPager
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                DisplayLinkToIndividualPages = false;
                ContainerDivClasses = null;
                UlElementClasses = new[] { "pager" };
                ClassToApplyToFirstListItemInPager = null;
                ClassToApplyToLastListItemInPager = null;
                LinkToPreviousPageFormat = "Previous";
                LinkToNextPageFormat = "Next";
            }
        }

        ///<summary>
        /// Twitter Bootstrap 2's basic pager format (just Previous and Next links), with aligned links.
        ///</summary>
        public bool PredefinedTwitterBootstrapPagerAligned
        {
            get { return true; }
            set
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Never;
                DisplayLinkToLastPage = PagedListDisplayMode.Never;
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
                DisplayLinkToNextPage = PagedListDisplayMode.Always;
                DisplayLinkToIndividualPages = false;
                ContainerDivClasses = null;
                UlElementClasses = new[] { "pager" };
                ClassToApplyToFirstListItemInPager = "previous";
                ClassToApplyToLastListItemInPager = "next";
                LinkToPreviousPageFormat = "&larr; Older";
                LinkToNextPageFormat = "Newer &rarr;";
            }
        }

        #endregion

        #region Helpers
        private string TagBuilderToString(TagBuilder tagBuilder)
        {
            var writer = new System.IO.StringWriter();
            tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            return writer.ToString();
        }

        private string TagBuilderToString(TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            var writer = new System.IO.StringWriter();
            tagBuilder.TagRenderMode = renderMode;
            tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            return writer.ToString();
        }

        private TagBuilder WrapInListItem(string text)
        {
            var li = new TagBuilder("li");
            li.InnerHtml.SetContent(text);
            return li;
        }

        private TagBuilder WrapInListItem(TagBuilder inner, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);
            if (FunctionToTransformEachPageLink != null)
                return FunctionToTransformEachPageLink(li, inner);
            li.InnerHtml.AppendHtml(TagBuilderToString(inner));
            return li;
        }

        private TagBuilder First(IPagedList list, Func<int, string> generatePageUrl)
        {
            const int targetPageNumber = 1;
            var first = new TagBuilder("a");
            first.InnerHtml.AppendHtml(string.Format(LinkToFirstPageFormat, targetPageNumber));

            foreach (var c in PageClasses ?? Enumerable.Empty<string>())
                first.AddCssClass(c);

            if (list.IsFirstPage)
                return WrapInListItem(first, "PagedList-skipToFirst", "disabled");

            first.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(first, "PagedList-skipToFirst");
        }

        private TagBuilder Previous(IPagedList list, Func<int, string> generatePageUrl)
        {
            var targetPageNumber = list.PageNumber - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml.AppendHtml(string.Format(LinkToPreviousPageFormat, targetPageNumber));
            previous.Attributes["rel"] = "prev";

            foreach (var c in PageClasses ?? Enumerable.Empty<string>())
                previous.AddCssClass(c);

            if (!list.HasPreviousPage)
                return WrapInListItem(previous, PreviousElementClass, "disabled");

            previous.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(previous, PreviousElementClass);
        }

        private TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl)
        {
            var format = FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = i == list.PageNumber ? new TagBuilder("span") : new TagBuilder("a");
            page.InnerHtml.SetContent(format(targetPageNumber));

            foreach (var c in PageClasses ?? Enumerable.Empty<string>())
                page.AddCssClass(c);

            if (i == list.PageNumber)
                return WrapInListItem(page, ActiveLiElementClass);

            page.Attributes["href"] = generatePageUrl(targetPageNumber);

            return WrapInListItem(page);
        }

        private TagBuilder Next(IPagedList list, Func<int, string> generatePageUrl)
        {
            var targetPageNumber = list.PageNumber + 1;
            var next = new TagBuilder("a");
            next.InnerHtml.AppendHtml(string.Format(LinkToNextPageFormat, targetPageNumber));
            next.Attributes["rel"] = "next";

            foreach (var c in PageClasses ?? Enumerable.Empty<string>())
                next.AddCssClass(c);

            if (!list.HasNextPage)
                return WrapInListItem(next, NextElementClass, "disabled");

            next.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(next, NextElementClass);
        }

        private TagBuilder Last(IPagedList list, Func<int, string> generatePageUrl)
        {
            var targetPageNumber = list.PageCount;
            var last = new TagBuilder("a");
            last.InnerHtml.AppendHtml(string.Format(LinkToLastPageFormat, targetPageNumber));

            foreach (var c in PageClasses ?? Enumerable.Empty<string>())
                last.AddCssClass(c);

            if (list.IsLastPage)
                return WrapInListItem(last, "PagedList-skipToLast", "disabled");

            last.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(last, "PagedList-skipToLast");
        }

        private TagBuilder PageCountAndLocationText(IPagedList list)
        {
            var text = new TagBuilder("a");
            text.InnerHtml.SetContent(string.Format(PageCountAndCurrentLocationFormat, list.PageNumber, list.PageCount));

            return WrapInListItem(text, "PagedList-pageCountAndLocation", "disabled");
        }

        private TagBuilder ItemSliceAndTotalText(IPagedList list)
        {
            var text = new TagBuilder("a");
            text.InnerHtml.SetContent(string.Format(ItemSliceAndTotalFormat, list.FirstItemOnPage, list.LastItemOnPage, list.TotalItemCount));

            return WrapInListItem(text, "PagedList-pageCountAndLocation", "disabled");
        }

        private TagBuilder Ellipses()
        {
            var a = new TagBuilder("a");
            a.InnerHtml.AppendHtml(EllipsesFormat);

            return WrapInListItem(a, EllipsesElementClass, "disabled");
        }

        #endregion

        ///<summary>
        /// The default settings render all navigation links and no descriptive text.
        ///</summary>
        public PagedListPagerTagHelper()
        {
            DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToIndividualPages = true;
            DisplayPageCountAndCurrentLocation = false;
            MaximumPageNumbersToDisplay = 10;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "&#8230;";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "«";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "»";
            LinkToLastPageFormat = "»»";
            PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            FunctionToDisplayEachPageNumber = null;
            ClassToApplyToFirstListItemInPager = null;
            ClassToApplyToLastListItemInPager = null;
            ContainerDivClasses = new[] { "pagination-container" };
            UlElementClasses = new[] { "pagination" };
            LiElementClasses = Enumerable.Empty<string>();
            PageClasses = Enumerable.Empty<string>();
            UlElementattributes = null;
            ActiveLiElementClass = "active";
            EllipsesElementClass = "PagedList-ellipses";
            PreviousElementClass = "PagedList-skipToPrevious";
            NextElementClass = "PagedList-skipToNext";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            if (List == null)
            {
                output.Content.SetContent("TagHelper attribute \"list\" is required!");
            }
            else if (Display == PagedListDisplayMode.Never || (Display == PagedListDisplayMode.IfNeeded && List.PageCount <= 1))
            {
                output.Content.SetContent("");
            }
            else
            {
                var listItemLinks = new List<TagBuilder>();

                //calculate start and end of range of page numbers
                var firstPageToDisplay = 1;
                var lastPageToDisplay = List.PageCount;
                var pageNumbersToDisplay = lastPageToDisplay;
                if (MaximumPageNumbersToDisplay.HasValue && List.PageCount > MaximumPageNumbersToDisplay)
                {
                    // cannot fit all pages into pager
                    var maxPageNumbersToDisplay = MaximumPageNumbersToDisplay.Value;
                    firstPageToDisplay = List.PageNumber - maxPageNumbersToDisplay / 2;
                    if (firstPageToDisplay < 1)
                        firstPageToDisplay = 1;
                    pageNumbersToDisplay = maxPageNumbersToDisplay;
                    lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                    if (lastPageToDisplay > List.PageCount)
                        firstPageToDisplay = List.PageCount - maxPageNumbersToDisplay + 1;
                }

                //first
                if (DisplayLinkToFirstPage == PagedListDisplayMode.Always || (DisplayLinkToFirstPage == PagedListDisplayMode.IfNeeded && firstPageToDisplay > 1))
                    listItemLinks.Add(First(List, PageUrl));

                //previous
                if (DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !List.IsFirstPage))
                    listItemLinks.Add(Previous(List, PageUrl));

                //text
                if (DisplayPageCountAndCurrentLocation)
                    listItemLinks.Add(PageCountAndLocationText(List));

                //text
                if (DisplayItemSliceAndTotal)
                    listItemLinks.Add(ItemSliceAndTotalText(List));

                //page
                if (DisplayLinkToIndividualPages)
                {
                    //if there are previous page numbers not displayed, show an ellipsis
                    if (DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                        listItemLinks.Add(Ellipses());

                    foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                    {
                        //show delimiter between page numbers
                        if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(DelimiterBetweenPageNumbers))
                            listItemLinks.Add(WrapInListItem(DelimiterBetweenPageNumbers));

                        //show page number link
                        listItemLinks.Add(Page(i, List, PageUrl));
                    }

                    //if there are subsequent page numbers not displayed, show an ellipsis
                    if (DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < List.PageCount)
                        listItemLinks.Add(Ellipses());
                }

                //next
                if (DisplayLinkToNextPage == PagedListDisplayMode.Always || (DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !List.IsLastPage))
                    listItemLinks.Add(Next(List, PageUrl));

                //last
                if (DisplayLinkToLastPage == PagedListDisplayMode.Always || (DisplayLinkToLastPage == PagedListDisplayMode.IfNeeded && lastPageToDisplay < List.PageCount))
                    listItemLinks.Add(Last(List, PageUrl));

                if (listItemLinks.Any())
                {
                    //append class to first item in list?
                    if (!string.IsNullOrWhiteSpace(ClassToApplyToFirstListItemInPager))
                        listItemLinks.First().AddCssClass(ClassToApplyToFirstListItemInPager);

                    //append class to last item in list?
                    if (!string.IsNullOrWhiteSpace(ClassToApplyToLastListItemInPager))
                        listItemLinks.Last().AddCssClass(ClassToApplyToLastListItemInPager);

                    //append classes to all list item links
                    foreach (var li in listItemLinks)
                        foreach (var c in LiElementClasses ?? Enumerable.Empty<string>())
                            li.AddCssClass(c);
                }

                //collapse all of the list items into one big string
                var listItemLinksString = listItemLinks.Aggregate(
                    new StringBuilder(),
                    (sb, listItem) => sb.Append(TagBuilderToString(listItem)),
                    sb => sb.ToString()
                    );

                var ul = new TagBuilder("ul");
                ul.InnerHtml.AppendHtml(listItemLinksString);
                foreach (var c in UlElementClasses ?? Enumerable.Empty<string>())
                    ul.AddCssClass(c);

                if (UlElementattributes != null)
                {
                    foreach (var c in UlElementattributes)
                        ul.MergeAttribute(c.Key, c.Value);
                }

                output.TagName = "div";
                foreach (var c in ContainerDivClasses ?? Enumerable.Empty<string>())
                    output.Attributes.Add("class", c);
                output.Content.AppendHtml(TagBuilderToString(ul));
            }
        }
    }
}
