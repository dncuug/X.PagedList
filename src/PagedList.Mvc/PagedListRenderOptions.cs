namespace PagedList.Mvc
{
	public class PagedListRenderOptions
	{
		public PagedListRenderOptions()
		{
			DisplayLinkToFirstPage = true;
			DisplayLinkToLastPage = true;
			DisplayLinkToIndividualPages = true;
			DisplayPageCountAndCurrentLocation = false;
			LinkToFirstPageFormat = "<< First";
			LinkToPreviousPageFormat = "< Previous";
			LinkToIndividualPageFormat = "{0}";
			LinkToNextPageFormat = "Next >";
			LinkToLastPageFormat = "Last >>";
			PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
			ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
		}

		public bool DisplayLinkToFirstPage { get; set; }
		public bool DisplayLinkToLastPage { get; set; }
		public bool DisplayLinkToIndividualPages { get; set; }
		public bool DisplayPageCountAndCurrentLocation { get; set; }
		public bool DisplayItemSliceAndTotal { get; set; }
		public string LinkToFirstPageFormat { get; set; }
		public string LinkToPreviousPageFormat { get; set; }
		public string LinkToIndividualPageFormat { get; set; }
		public string LinkToNextPageFormat { get; set; }
		public string LinkToLastPageFormat { get; set; }
		public string PageCountAndCurrentLocationFormat { get; set; }
		public string ItemSliceAndTotalFormat { get; set; }

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
	}
}