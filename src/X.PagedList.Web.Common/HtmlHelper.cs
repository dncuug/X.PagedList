using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X.PagedList.Web.Common
{
    public sealed class HtmlHelper
    {
        private readonly ITagBuilderFactory _tagBuilderFactory;

        public HtmlHelper(ITagBuilderFactory tagBuilderFactory)
        {
            _tagBuilderFactory = tagBuilderFactory;
        }

        #region Private methods

        private static void SetInnerText(ITagBuilder tagBuilder, string innerText)
        {
            tagBuilder.SetInnerText(innerText);
        }

        private static void AppendHtml(ITagBuilder tagBuilder, string innerHtml)
        {
            tagBuilder.AppendHtml(innerHtml);
        }

        private static string TagBuilderToString(ITagBuilder tagBuilder)
        {
            return tagBuilder
                .ToString(TagRenderMode.Normal);
        }

        private static string TagBuilderToString(ITagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return tagBuilder
                .ToString(renderMode);
        }

        private ITagBuilder WrapInListItem(string text)
        {
            var li = _tagBuilderFactory
                .Create("li");

            SetInnerText(li, text);

            return li;
        }

        private ITagBuilder WrapInListItem(ITagBuilder inner, PagedListRenderOptions options, params string[] classes)
        {
            var li = _tagBuilderFactory
                .Create("li");

            foreach (var @class in classes)
            {
                li.AddCssClass(@class);
            }

            if (options != null)
            {
                if (options.FunctionToTransformEachPageLink != null)
                {
                    return options.FunctionToTransformEachPageLink(li, inner);
                }
            }

            AppendHtml(li, TagBuilderToString(inner));
            return li;
        }

        private ITagBuilder First(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            const int targetPageNumber = 1;
            var first = _tagBuilderFactory
                .Create("a");

            AppendHtml(first, string.Format(options.LinkToFirstPageFormat, targetPageNumber));

            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
            {
                first.AddCssClass(c);
            }

            if (list.IsFirstPage)
            {
                return WrapInListItem(first, options, "PagedList-skipToFirst", "disabled");
            }

            first.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(first, options, "PagedList-skipToFirst");
        }

        private ITagBuilder Previous(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageNumber - 1;
            var previous = _tagBuilderFactory
                .Create("a");

            AppendHtml(previous, string.Format(options.LinkToPreviousPageFormat, targetPageNumber));

            previous.Attributes.Add("rel", "prev");

            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
            {
                previous.AddCssClass(c);
            }

            if (!list.HasPreviousPage)
            {
                return WrapInListItem(previous, options, options.PreviousElementClass, "disabled");
            }

            previous.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(previous, options, options.PreviousElementClass);
        }

        private ITagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = i == list.PageNumber
                ? _tagBuilderFactory
                    .Create("span")
                : _tagBuilderFactory
                    .Create("a");

            SetInnerText(page, format(targetPageNumber));

            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
            {
                page.AddCssClass(c);
            }

            if (i == list.PageNumber)
            {
                return WrapInListItem(page, options, options.ActiveLiElementClass);
            }

            page.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(page, options);
        }

        private ITagBuilder Next(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageNumber + 1;
            var next = _tagBuilderFactory
                .Create("a");

            AppendHtml(next, string.Format(options.LinkToNextPageFormat, targetPageNumber));

            next.Attributes.Add("rel", "next");

            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
            {
                next.AddCssClass(c);
            }

            if (!list.HasNextPage)
            {
                return WrapInListItem(next, options, options.NextElementClass, "disabled");
            }

            next.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(next, options, options.NextElementClass);
        }

        private ITagBuilder Last(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageCount;
            var last = _tagBuilderFactory
                .Create("a");

            AppendHtml(last, string.Format(options.LinkToLastPageFormat, targetPageNumber));

            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
            {
                last.AddCssClass(c);
            }

            if (list.IsLastPage)
            {
                return WrapInListItem(last, options, "PagedList-skipToLast", "disabled");
            }

            last.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(last, options, "PagedList-skipToLast");
        }

        private ITagBuilder PageCountAndLocationText(IPagedList list, PagedListRenderOptions options)
        {
            var text = _tagBuilderFactory
                .Create("a");

            SetInnerText(text, string.Format(options.PageCountAndCurrentLocationFormat, list.PageNumber, list.PageCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private ITagBuilder ItemSliceAndTotalText(IPagedList list, PagedListRenderOptions options)
        {
            var text = _tagBuilderFactory
                .Create("a");

            SetInnerText(text, string.Format(options.ItemSliceAndTotalFormat, list.FirstItemOnPage, list.LastItemOnPage, list.TotalItemCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private ITagBuilder PreviousEllipsis(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options, int firstPageToDisplay)
        {
            var previous = _tagBuilderFactory
                .Create("a");

            AppendHtml(previous, options.EllipsesFormat);

            previous.Attributes.Add("rel", "prev");
            previous.AddCssClass("PagedList-skipToPrevious");

            if (!list.HasPreviousPage)
            {
                return WrapInListItem(previous, options, options.EllipsesElementClass, "disabled");
            }

            var targetPageNumber = firstPageToDisplay - 1;

            previous.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(previous, options, options.EllipsesElementClass);
        }

        private ITagBuilder NextEllipsis(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options, int lastPageToDisplay)
        {
            var next = _tagBuilderFactory
                .Create("a");

            AppendHtml(next, options.EllipsesFormat);

            next.Attributes.Add("rel", "next");
            next.AddCssClass("PagedList-skipToNext");

            if (!list.HasNextPage)
            {
                return WrapInListItem(next, options, options.EllipsesElementClass, "disabled");
            }

            var targetPageNumber = lastPageToDisplay + 1;

            next.Attributes.Add("href", generatePageUrl(targetPageNumber));

            return WrapInListItem(next, options, options.EllipsesElementClass);
        }

        #endregion Private methods

        public string PagedListPager(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            if (options.Display == PagedListDisplayMode.Never || (options.Display == PagedListDisplayMode.IfNeeded && list.PageCount <= 1))
            {
                return null;
            }

            var listItemLinks = new List<ITagBuilder>();

            //calculate start and end of range of page numbers
            var firstPageToDisplay = 1;
            var lastPageToDisplay = list.PageCount;
            var pageNumbersToDisplay = lastPageToDisplay;

            if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
            {
                // cannot fit all pages into pager
                var maxPageNumbersToDisplay = options.MaximumPageNumbersToDisplay.Value;

                firstPageToDisplay = list.PageNumber - maxPageNumbersToDisplay / 2;

                if (firstPageToDisplay < 1)
                {
                    firstPageToDisplay = 1;
                }

                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;

                if (lastPageToDisplay > list.PageCount)
                {
                    firstPageToDisplay = list.PageCount - maxPageNumbersToDisplay + 1;
                }
            }

            //first
            if (options.DisplayLinkToFirstPage == PagedListDisplayMode.Always ||
                (options.DisplayLinkToFirstPage == PagedListDisplayMode.IfNeeded && firstPageToDisplay > 1))
            {
                listItemLinks.Add(First(list, generatePageUrl, options));
            }

            //previous
            if (options.DisplayLinkToPreviousPage == PagedListDisplayMode.Always ||
                (options.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !list.IsFirstPage))
            {
                listItemLinks.Add(Previous(list, generatePageUrl, options));
            }

            //text
            if (options.DisplayPageCountAndCurrentLocation)
            {
                listItemLinks.Add(PageCountAndLocationText(list, options));
            }

            //text
            if (options.DisplayItemSliceAndTotal)
            {
                listItemLinks.Add(ItemSliceAndTotalText(list, options));
            }

            //page
            if (options.DisplayLinkToIndividualPages)
            {
                //if there are previous page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                {
                    listItemLinks.Add(PreviousEllipsis(list, generatePageUrl, options, firstPageToDisplay));
                }

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    //show delimiter between page numbers
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(options.DelimiterBetweenPageNumbers))
                    {
                        listItemLinks.Add(WrapInListItem(options.DelimiterBetweenPageNumbers));
                    }

                    //show page number link
                    listItemLinks.Add(Page(i, list, generatePageUrl, options));
                }

                //if there are subsequent page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers &&
                    (firstPageToDisplay + pageNumbersToDisplay - 1) < list.PageCount)
                {
                    listItemLinks.Add(NextEllipsis(list, generatePageUrl, options, lastPageToDisplay));
                }
            }

            //next
            if (options.DisplayLinkToNextPage == PagedListDisplayMode.Always ||
                (options.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !list.IsLastPage))
            {
                listItemLinks.Add(Next(list, generatePageUrl, options));
            }

            //last
            if (options.DisplayLinkToLastPage == PagedListDisplayMode.Always ||
                (options.DisplayLinkToLastPage == PagedListDisplayMode.IfNeeded && lastPageToDisplay < list.PageCount))
            {
                listItemLinks.Add(Last(list, generatePageUrl, options));
            }

            if (listItemLinks.Any())
            {
                //append class to first item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToFirstListItemInPager))
                {
                    listItemLinks.First().AddCssClass(options.ClassToApplyToFirstListItemInPager);
                }

                //append class to last item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToLastListItemInPager))
                {
                    listItemLinks.Last().AddCssClass(options.ClassToApplyToLastListItemInPager);
                }

                //append classes to all list item links
                foreach (var li in listItemLinks)
                {
                    foreach (var c in options.LiElementClasses ?? Enumerable.Empty<string>())
                    {
                        li.AddCssClass(c);
                    }
                }
            }

            //collapse all of the list items into one big string
            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(TagBuilderToString(listItem)),
                sb => sb.ToString());

            var ul = _tagBuilderFactory
                .Create("ul");

            AppendHtml(ul, listItemLinksString);

            foreach (var c in options.UlElementClasses ?? Enumerable.Empty<string>())
            {
                ul.AddCssClass(c);
            }

            if (options.UlElementattributes != null)
            {
                foreach (var c in options.UlElementattributes)
                {
                    ul.MergeAttribute(c.Key, c.Value);
                }
            }

            var outerDiv = _tagBuilderFactory
                .Create("div");

            foreach (var c in options.ContainerDivClasses ?? Enumerable.Empty<string>())
            {
                outerDiv.AddCssClass(c);
            }

            AppendHtml(outerDiv, TagBuilderToString(ul));

            return TagBuilderToString(outerDiv);
        }

        public string PagedListGoToPageForm(IPagedList list, string formAction, GoToFormRenderOptions options)
        {
            var form = _tagBuilderFactory
                .Create("form");

            form.AddCssClass("PagedList-goToPage");
            form.Attributes.Add("action", formAction);
            form.Attributes.Add("method", "get");

            var fieldset = _tagBuilderFactory
                .Create("fieldset");

            var label = _tagBuilderFactory
                .Create("label");

            label.Attributes.Add("for", options.InputFieldName);

            SetInnerText(label, options.LabelFormat);

            var input = _tagBuilderFactory
                .Create("input");

            input.Attributes.Add("type", options.InputFieldType);
            input.Attributes.Add("name", options.InputFieldName);
            input.Attributes.Add("value", list.PageNumber.ToString());
            input.Attributes.Add("class", options.InputFieldClass);
            input.Attributes.Add("Style", $"width: {options.InputWidth}px");

            var submit = _tagBuilderFactory
                .Create("input");

            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", options.SubmitButtonFormat);
            submit.Attributes.Add("class", options.SubmitButtonClass);
            submit.Attributes.Add("Style", $"width: {options.SubmitButtonWidth}px");

            AppendHtml(fieldset, TagBuilderToString(label));
            AppendHtml(fieldset, TagBuilderToString(input, TagRenderMode.SelfClosing));
            AppendHtml(fieldset, TagBuilderToString(submit, TagRenderMode.SelfClosing));
            AppendHtml(form, TagBuilderToString(fieldset));

            return TagBuilderToString(form);
        }
    }
}
