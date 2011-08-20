using System;
using System.Collections;
using System.Collections.Generic;

namespace PagedList
{
	/// <summary>
	/// 	Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </summary>
	/// <remarks>
	/// 	Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </remarks>
	/// <typeparam name = "T">The type of object the collection should contain.</typeparam>
	/// <seealso cref = "IPagedList{T}" />
	/// <seealso cref = "List{T}" />
	public abstract class BasePagedList<T> : PagedListMetaData, IPagedList<T>
	{
		/// <summary>
		/// 	The subset of items contained only within this one page of the superset.
		/// </summary>
		protected readonly List<T> Subset = new List<T>();

		/// <summary>
		/// 	Initializes a new instance of a type deriving from <see cref = "BasePagedList{T}" /> and sets properties needed to calculate position and size data on the subset and superset.
		/// </summary>
		/// <param name = "pageNumber">The one-based index of the subset of objects contained by this instance.</param>
		/// <param name = "pageSize">The maximum size of any individual subset.</param>
		/// <param name = "totalItemCount">The size of the superset.</param>
		protected internal BasePagedList(int pageNumber, int pageSize, int totalItemCount)
		{
			if (pageNumber < 1)
				throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
			if (pageSize < 1)
				throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

			// set source to blank list if superset is null to prevent exceptions
			TotalItemCount = totalItemCount;
			PageSize = pageSize;
			PageNumber = pageNumber;
			PageCount = TotalItemCount > 0
			            	? (int) Math.Ceiling(TotalItemCount/(double) PageSize)
			            	: 0;
			HasPreviousPage = PageNumber > 1;
			HasNextPage = PageNumber < PageCount;
			IsFirstPage = PageNumber == 1;
			IsLastPage = PageNumber >= PageCount;
			FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
			var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
			LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
			                 	? TotalItemCount
			                 	: numberOfLastItemOnPage;
		}

		#region IPagedList<T> Members

		/// <summary>
		/// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
		/// </summary>
		/// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return Subset.GetEnumerator();
		}

		/// <summary>
		/// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
		/// </summary>
		/// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		///<summary>
		///	Gets the element at the specified index.
		///</summary>
		///<param name = "index">The zero-based index of the element to get.</param>
		public T this[int index]
		{
			get { return Subset[index]; }
		}

		/// <summary>
		/// 	Gets the number of elements contained on this page.
		/// </summary>
		public int Count
		{
			get { return Subset.Count; }
		}

		///<summary>
		/// Gets a non-enumerable copy of this paged list.
		///</summary>
		///<returns>A non-enumerable copy of this paged list.</returns>
		public IPagedList GetMetaData()
		{
			return new PagedListMetaData(this);
		}

		#endregion
	}
}