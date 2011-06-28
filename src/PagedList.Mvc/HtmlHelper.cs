using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PagedList.Mvc
{
	///<summary>
	///	Extension methods for generating paging controls that can operate on instances of IPagedList.
	///</summary>
	public static class HtmlHelper
	{
		private static TagBuilder WrapInListItem(TagBuilder inner, params string[] classes)
		{
			var li = new TagBuilder("li");
			foreach (var @class in classes)
				li.AddCssClass(@class);
			li.InnerHtml = inner.ToString();
			return li;
		}

		private static TagBuilder First(IPagedList list, Func<int, string> generatePageUrl, string format)
		{
			const int targetPageIndex = 0;
			var first = new TagBuilder("a");
			first.SetInnerText(string.Format(format, targetPageIndex + 1));

			if (list.IsFirstPage)
				return WrapInListItem(first, "PagedList-skipToFirst", "PagedList-disabled");

			first.Attributes["href"] = generatePageUrl(targetPageIndex);
			return WrapInListItem(first, "PagedList-skipToFirst");
		}

		private static TagBuilder Previous(IPagedList list, Func<int, string> generatePageUrl, string format)
		{
			var targetPageIndex = list.PageIndex - 1;
			var previous = new TagBuilder("a");
			previous.SetInnerText(string.Format(format, targetPageIndex + 1));

			if (!list.HasPreviousPage)
				return WrapInListItem(previous, "PagedList-skipToPrevious", "PagedList-disabled");

			previous.Attributes["href"] = generatePageUrl(targetPageIndex);
			return WrapInListItem(previous, "PagedList-skipToPrevious");
		}

		private static TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, string format)
		{
			return Page(i, list, generatePageUrl, (pageNumber => string.Format(format, pageNumber)));
		}

		private static TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, Func<int, string> format)
		{
			var targetPageIndex = i;
			var page = new TagBuilder("a");
			page.SetInnerText(format(targetPageIndex + 1));

			if (i == list.PageIndex)
				return WrapInListItem(page, "PagedList-skipToPage", "PagedList-currentPage", "PagedList-disabled");

			page.Attributes["href"] = generatePageUrl(targetPageIndex);
			return WrapInListItem(page, "PagedList-skipToPage");
		}

		private static TagBuilder Next(IPagedList list, Func<int, string> generatePageUrl, string format)
		{
			var targetPageIndex = list.PageIndex + 1;
			var next = new TagBuilder("a");
			next.SetInnerText(string.Format(format, targetPageIndex + 1));

			if (!list.HasNextPage)
				return WrapInListItem(next, "PagedList-skipToNext", "PagedList-disabled");

			next.Attributes["href"] = generatePageUrl(targetPageIndex);
			return WrapInListItem(next, "PagedList-skipToNext");
		}

		private static TagBuilder Last(IPagedList list, Func<int, string> generatePageUrl, string format)
		{
			var targetPageIndex = list.PageCount - 1;
			var last = new TagBuilder("a");
			last.SetInnerText(string.Format(format, targetPageIndex + 1));

			if (list.IsLastPage)
				return WrapInListItem(last, "PagedList-skipToLast", "PagedList-disabled");

			last.Attributes["href"] = generatePageUrl(targetPageIndex);
			return WrapInListItem(last, "PagedList-skipToLast");
		}

		private static TagBuilder PageCountAndLocationText(IPagedList list, string format)
		{
			var text = new TagBuilder("span");
			text.SetInnerText(string.Format(format, list.PageNumber, list.PageCount));

			return WrapInListItem(text, "PagedList-pageCountAndLocation");
		}

		private static TagBuilder ItemSliceAndTotalText(IPagedList list, string format)
		{
			var text = new TagBuilder("span");
			text.SetInnerText(string.Format(format, list.FirstItemOnPage, list.LastItemOnPage, list.TotalItemCount));

			return WrapInListItem(text, "PagedList-pageCountAndLocation");
		}

		private static TagBuilder Ellipses(string format)
		{
			var text = new TagBuilder("span");
			text.SetInnerText(format); //TODO: fix so we can use &#8230;

			return WrapInListItem(text, "PagedList-ellipses");
		}

		///<summary>
		///	Displays a configurable paging control for instances of PagedList.
		///</summary>
		///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
		///<param name = "list">The PagedList to use as the data source.</param>
		///<param name = "generatePageUrl">A function that takes the index of the desired page and returns a URL-string that will load that page.</param>
		///<returns>Outputs the paging control HTML.</returns>
		public static MvcHtmlString PagedListPager(this System.Web.Mvc.HtmlHelper html,
												   IPagedList list,
												   Func<int, string> generatePageUrl)
		{
			return PagedListPager(html, list, generatePageUrl, new PagedListRenderOptions());
		}

		///<summary>
		///	Displays a configurable paging control for instances of PagedList.
		///</summary>
		///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
		///<param name = "list">The PagedList to use as the data source.</param>
		///<param name = "generatePageUrl">A function that takes the index of the desired page and returns a URL-string that will load that page.</param>
		///<param name = "options">Formatting options.</param>
		///<returns>Outputs the paging control HTML.</returns>
		public static MvcHtmlString PagedListPager(this System.Web.Mvc.HtmlHelper html,
												   IPagedList list,
												   Func<int, string> generatePageUrl,
												   PagedListRenderOptions options)
		{
			var listItemLinks = new StringBuilder();

			//first
			if (options.DisplayLinkToFirstPage)
				listItemLinks.Append(First(list, generatePageUrl, options.LinkToFirstPageFormat));

			//previous
			if (options.DisplayLinkToPreviousPage)
				listItemLinks.Append(Previous(list, generatePageUrl, options.LinkToPreviousPageFormat));

			//text
			if (options.DisplayPageCountAndCurrentLocation)
				listItemLinks.Append(PageCountAndLocationText(list, options.PageCountAndCurrentLocationFormat));

			//text
			if (options.DisplayItemSliceAndTotal)
				listItemLinks.Append(ItemSliceAndTotalText(list, options.ItemSliceAndTotalFormat));

			//page
			if (options.DisplayLinkToIndividualPages)
			{
				var start = 0;
				var end = list.PageCount;
				if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
				{
					start = list.PageIndex - options.MaximumPageNumbersToDisplay.Value / 2;
					if (start < 0)
						start = 0;
					end = options.MaximumPageNumbersToDisplay.Value;
					if ((start + end) > list.PageCount)
						start = list.PageCount - options.MaximumPageNumbersToDisplay.Value;
				}

				if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && start > 0)
					listItemLinks.Append(Ellipses(options.EllipsesFormat));

                foreach (var i in Enumerable.Range(start, end))
                {
                    if (ADelimiterWasSet(options) && ThisIsNotTheFirstPage(start, i))
                        listItemLinks.Append(options.Delimiter);

                    listItemLinks.Append(options.FunctionToDisplayEachPageNumber == null
                                             ? Page(i, list, generatePageUrl, options.LinkToIndividualPageFormat)
                                             : Page(i, list, generatePageUrl, options.FunctionToDisplayEachPageNumber));
                }

			    if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && (start + end) < list.PageCount)
					listItemLinks.Append(Ellipses(options.EllipsesFormat));
			}

			//next
			if (options.DisplayLinkToNextPage)
				listItemLinks.Append(Next(list, generatePageUrl, options.LinkToNextPageFormat));

			//last
			if (options.DisplayLinkToLastPage)
				listItemLinks.Append(Last(list, generatePageUrl, options.LinkToLastPageFormat));

			var ul = new TagBuilder("ul")
						{
							InnerHtml = listItemLinks.ToString()
						};

			var outerDiv = new TagBuilder("div");
			outerDiv.AddCssClass("PagedList-pager");
			outerDiv.InnerHtml = ul.ToString();

			return new MvcHtmlString(outerDiv.ToString());
		}

	    private static bool ThisIsNotTheFirstPage(int start, int i)
	    {
	        return (i > start);
	    }

	    private static bool ADelimiterWasSet(PagedListRenderOptions options)
	    {
	        return (string.IsNullOrEmpty(options.Delimiter) == false);
	    }
	}
}