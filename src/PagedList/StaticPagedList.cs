using System;
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
	/// <seealso cref="IPagedList{T}"/>
	/// <seealso cref="BasePagedList{T}"/>
	/// <seealso cref="PagedList{T}"/>
	/// <seealso cref="List{T}"/>
	public class StaticPagedList<T> : BasePagedList<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StaticPagedList{T}"/> class that contains the already divided subset and information about the size of the superset and the subset's position within it.
		/// </summary>
		/// <param name="subset">The single subset this collection should represent.</param>
		/// <param name="index">The index of the subset of objects contained by this instance.</param>
		/// <param name="pageSize">The maximum size of any individual subset.</param>
		/// <param name="totalItemCount">The size of the superset.</param>
		/// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
		public StaticPagedList(IEnumerable<T> subset, int index, int pageSize, int totalItemCount)
			: base(index, pageSize, totalItemCount)
		{
			Subset.AddRange(subset);
		}
	}
}