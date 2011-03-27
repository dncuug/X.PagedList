using System.Collections.Generic;

namespace PagedList
{
	/// <summary>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </summary>
	/// <remarks>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </remarks>
	/// <typeparam name="T">The type of object the collection should contain.</typeparam>
	/// <seealso cref="IList{T}"/>
	public interface IPagedList<T> : IPagedList, IList<T>
	{
	}

	/// <summary>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </summary>
	/// <remarks>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </remarks>
	public interface IPagedList
	{
		/// <summary>
		/// Total number of subsets within the superset.
		/// </summary>
		/// <value>
		/// Total number of subsets within the superset.
		/// </value>
		int PageCount { get; }

		/// <summary>
		/// Total number of objects contained within the superset.
		/// </summary>
		/// <value>
		/// Total number of objects contained within the superset.
		/// </value>
		int TotalItemCount { get; }

		/// <summary>
		/// Zero-based index of this subset within the superset.
		/// </summary>
		/// <value>
		/// Zero-based index of this subset within the superset.
		/// </value>
		int PageIndex { get; }

		/// <summary>
		/// One-based index of this subset within the superset.
		/// </summary>
		/// <value>
		/// One-based index of this subset within the superset.
		/// </value>
		int PageNumber { get; }

		/// <summary>
		/// Maximum size any individual subset.
		/// </summary>
		/// <value>
		/// Maximum size any individual subset.
		/// </value>
		int PageSize { get; }

		/// <summary>
		/// Returns true if this is NOT the first subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is NOT the first subset within the superset.
		/// </value>
		bool HasPreviousPage { get; }

		/// <summary>
		/// Returns true if this is NOT the last subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is NOT the last subset within the superset.
		/// </value>
		bool HasNextPage { get; }

		/// <summary>
		/// Returns true if this is the first subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is the first subset within the superset.
		/// </value>
		bool IsFirstPage { get; }

		/// <summary>
		/// Returns true if this is the last subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is the last subset within the superset.
		/// </value>
		bool IsLastPage { get; }

		/// <summary>
		/// One-based index of the first item in the paged subset.
		/// </summary>
		/// <value>
		/// One-based index of the first item in the paged subset.
		/// </value>
		int FirstItemOnPage { get; }

		/// <summary>
		/// One-based index of the last item in the paged subset.
		/// </summary>
		/// <value>
		/// One-based index of the last item in the paged subset.
		/// </value>
		int LastItemOnPage { get; }
	}
}