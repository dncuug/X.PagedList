namespace PagedList
{
	///<summary>
	/// Non-enumerable version of the PagedList class.
	///</summary>
	public class PagedListMetaData : IPagedList
	{
		/// <summary>
		/// Protected constructor that allows for instantiation without passing in a separate list.
		/// </summary>
		protected PagedListMetaData()
		{
		}

		///<summary>
		/// Non-enumerable version of the PagedList class.
		///</summary>
		///<param name="pagedList">A PagedList (likely enumerable) to copy metadata from.</param>
		public PagedListMetaData(IPagedList pagedList)
		{
			PageCount = pagedList.PageCount;
			TotalItemCount = pagedList.TotalItemCount;
			PageNumber = pagedList.PageNumber;
			PageSize = pagedList.PageSize;
			HasPreviousPage = pagedList.HasPreviousPage;
			HasNextPage = pagedList.HasNextPage;
			IsFirstPage = pagedList.IsFirstPage;
			IsLastPage = pagedList.IsLastPage;
			FirstItemOnPage = pagedList.FirstItemOnPage;
			LastItemOnPage = pagedList.LastItemOnPage;
		}

		#region IPagedList Members

		/// <summary>
		/// 	Total number of subsets within the superset.
		/// </summary>
		/// <value>
		/// 	Total number of subsets within the superset.
		/// </value>
		public int PageCount { get; protected set; }

		/// <summary>
		/// 	Total number of objects contained within the superset.
		/// </summary>
		/// <value>
		/// 	Total number of objects contained within the superset.
		/// </value>
		public int TotalItemCount { get; protected set; }
		
		/// <summary>
		/// 	One-based index of this subset within the superset.
		/// </summary>
		/// <value>
		/// 	One-based index of this subset within the superset.
		/// </value>
		public int PageNumber { get; protected set; }

		/// <summary>
		/// 	Maximum size any individual subset.
		/// </summary>
		/// <value>
		/// 	Maximum size any individual subset.
		/// </value>
		public int PageSize { get; protected set; }

		/// <summary>
		/// 	Returns true if this is NOT the first subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if this is NOT the first subset within the superset.
		/// </value>
		public bool HasPreviousPage { get; protected set; }

		/// <summary>
		/// 	Returns true if this is NOT the last subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if this is NOT the last subset within the superset.
		/// </value>
		public bool HasNextPage { get; protected set; }

		/// <summary>
		/// 	Returns true if this is the first subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if this is the first subset within the superset.
		/// </value>
		public bool IsFirstPage { get; protected set; }

		/// <summary>
		/// 	Returns true if this is the last subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if this is the last subset within the superset.
		/// </value>
		public bool IsLastPage { get; protected set; }

		/// <summary>
		/// 	One-based index of the first item in the paged subset.
		/// </summary>
		/// <value>
		/// 	One-based index of the first item in the paged subset.
		/// </value>
		public int FirstItemOnPage { get; protected set; }

		/// <summary>
		/// 	One-based index of the last item in the paged subset.
		/// </summary>
		/// <value>
		/// 	One-based index of the last item in the paged subset.
		/// </value>
		public int LastItemOnPage { get; protected set; }

		#endregion
	}
}