using System;

namespace PagedList.Mvc
{
	///<summary>
	/// Options for configuring the output of <see cref = "HtmlHelper" />.
	///</summary>
	public class PagedListRenderOptions
	{
		///<summary>
		/// The default settings render all navigation links and no descriptive text.
		///</summary>
		public PagedListRenderOptions()
		{
			DisplayLinkToFirstPage = false;
			DisplayLinkToLastPage = false;
			DisplayLinkToPreviousPage = true;
			DisplayLinkToNextPage = true;
			DisplayLinkToIndividualPages = true;
			DisplayPageCountAndCurrentLocation = false;
			MaximumPageNumbersToDisplay = 10;
			DisplayEllipsesWhenNotShowingAllPageNumbers = true;
			EllipsesFormat = "&#8230;";
			LinkToFirstPageFormat = "&larr;&larr; First";
			LinkToPreviousPageFormat = "&larr; Previous";
			LinkToIndividualPageFormat = "{0}";
			LinkToNextPageFormat = "Next &rarr;";
			LinkToLastPageFormat = "Last &rarr;&rarr;";
			PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
			ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
			FunctionToDisplayEachPageNumber = null;
			ClassToApplyToLastListItemInPager = "next";
		}

		///<summary>
		/// Specifies a class to append to the last list item in the pager. If null or whitespace is defined, no additional class is added to last list item in list.
		///</summary>
		public string ClassToApplyToLastListItemInPager { get; set; }

		///<summary>
		/// When true, includes a hyperlink to the first page of the list.
		///</summary>
		public bool DisplayLinkToFirstPage { get; set; }

		///<summary>
		/// When true, includes a hyperlink to the last page of the list.
		///</summary>
		public bool DisplayLinkToLastPage { get; set; }

		///<summary>
		/// When true, includes a hyperlink to the previous page of the list.
		///</summary>
		public bool DisplayLinkToPreviousPage { get; set; }

		///<summary>
		/// When true, includes a hyperlink to the next page of the list.
		///</summary>
		public bool DisplayLinkToNextPage { get; set; }

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

		///<summary>
		/// Also includes links to First and Last pages.
		///</summary>
		public static PagedListRenderOptions DefaultPlusFirstAndLast
		{
			get
			{
				return new PagedListRenderOptions
				{
					DisplayLinkToFirstPage = true,
					DisplayLinkToLastPage = true,
				};
			}
		}

		///<summary>
		/// Shows only the Previous and Next links.
		///</summary>
		public static PagedListRenderOptions Minimal
		{
			get
			{
				return new PagedListRenderOptions
						{
							DisplayLinkToFirstPage = false,
							DisplayLinkToLastPage = false,
							DisplayLinkToIndividualPages = false
						};
			}
		}

		///<summary>
		/// Shows Previous and Next links along with current page number and page count.
		///</summary>
		public static PagedListRenderOptions MinimalWithPageCountText
		{
			get
			{
				return new PagedListRenderOptions
				{
					DisplayLinkToFirstPage = false,
					DisplayLinkToLastPage = false,
					DisplayLinkToIndividualPages = false,
					DisplayPageCountAndCurrentLocation = true
				};
			}
		}

		///<summary>
		/// Shows Previous and Next links along with index of first and last items on page and total number of items across all pages.
		///</summary>
		public static PagedListRenderOptions MinimalWithItemCountText
		{
			get
			{
				return new PagedListRenderOptions
				{
					DisplayLinkToFirstPage = false,
					DisplayLinkToLastPage = false,
					DisplayLinkToIndividualPages = false,
					DisplayItemSliceAndTotal = true
				};
			}
		}

		///<summary>
		/// Shows only links to each individual page.
		///</summary>
		public static PagedListRenderOptions PageNumbersOnly
		{
			get
			{
				return new PagedListRenderOptions
				{
					DisplayLinkToFirstPage = false,
					DisplayLinkToLastPage = false,
					DisplayLinkToPreviousPage = false,
					DisplayLinkToNextPage = false,
					DisplayEllipsesWhenNotShowingAllPageNumbers = false
				};
			}
		}

		///<summary>
		/// Shows Next and Previous while limiting to a max of 5 page numbers at a time.
		///</summary>
		public static PagedListRenderOptions OnlyShowFivePagesAtATime
		{
			get
			{
				return new PagedListRenderOptions
				{
					DisplayLinkToFirstPage = false,
					DisplayLinkToLastPage = false,
					DisplayLinkToPreviousPage = true,
					DisplayLinkToNextPage = true,
					MaximumPageNumbersToDisplay = 5
				};
			}
		}
	}
}