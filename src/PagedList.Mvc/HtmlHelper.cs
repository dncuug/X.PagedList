using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PagedList.Mvc
{
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
			var targetPageIndex = i;
			var page = new TagBuilder("a");
			page.SetInnerText(string.Format(format, targetPageIndex + 1));

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

		public static MvcHtmlString PagedListPager(this System.Web.Mvc.HtmlHelper html,
		                                           IPagedList list,
		                                           Func<int, string> generatePageUrl)
		{
			return PagedListPager(html, list, generatePageUrl, new PagedListRenderOptions());
		}

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
			listItemLinks.Append(Previous(list, generatePageUrl, options.LinkToPreviousPageFormat));

			//text
			if (options.DisplayPageCountAndCurrentLocation)
				listItemLinks.Append(PageCountAndLocationText(list, options.PageCountAndCurrentLocationFormat));

			//text
			if (options.DisplayItemSliceAndTotal)
				listItemLinks.Append(ItemSliceAndTotalText(list, options.ItemSliceAndTotalFormat));

			//page
			if (options.DisplayLinkToIndividualPages)
				foreach (var i in Enumerable.Range(0, list.PageCount))
					listItemLinks.Append(Page(i, list, generatePageUrl, options.LinkToIndividualPageFormat));

			//next
			listItemLinks.Append(Next(list, generatePageUrl, options.LinkToNextPageFormat));

			//last
			if (options.DisplayLinkToLastPage)
				listItemLinks.Append(Last(list, generatePageUrl, options.LinkToLastPageFormat));

			var ul = new TagBuilder("ul")
			         	{
			         		InnerHtml = listItemLinks.ToString()
			         	};

			var outerDiv = new TagBuilder("div");
			outerDiv.AddCssClass("PagedList-control");
			outerDiv.InnerHtml = ul.ToString();

			return new MvcHtmlString(outerDiv.ToString());
		}
	}
}