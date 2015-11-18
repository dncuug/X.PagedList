using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace PagedList
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <remarks>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </remarks>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <seealso cref="IPagedList{T}"/>
    /// <seealso cref="BasePagedList{T}"/>
    /// <seealso cref="StaticPagedList{T}"/>
    /// <seealso cref="List{T}"/>
    //[Serializable]        
    public class PagedListForEntityFramework<T, TKey> : PagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="keySelector">Expression for Order</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedListForEntityFramework(IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber",  "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize","PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = superset == null ? 0 : superset.Count();
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0
                        ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
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

            // add items to internal list
            if (superset != null && TotalItemCount > 0)
                Subset.AddRange(pageNumber == 1
                    ? superset.Take(pageSize).ToList()
                    : superset.OrderBy(keySelector).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// For Clone PagedList
        /// </summary>
        /// <param name="pagedListMetaData">Source PagedList</param>
        /// <param name="superset">Source collection</param>
        public PagedListForEntityFramework(PagedListMetaData pagedListMetaData, IEnumerable<T> superset)
        {
            TotalItemCount = pagedListMetaData.TotalItemCount;
            PageSize = pagedListMetaData.PageSize;
            PageNumber = pagedListMetaData.PageNumber;
            PageCount = pagedListMetaData.PageCount;
            HasPreviousPage = pagedListMetaData.HasPreviousPage;
            HasNextPage = pagedListMetaData.HasNextPage;
            IsFirstPage = pagedListMetaData.IsFirstPage;
            IsLastPage = pagedListMetaData.IsLastPage;
            FirstItemOnPage = pagedListMetaData.FirstItemOnPage;
            LastItemOnPage = pagedListMetaData.LastItemOnPage;

            Subset.AddRange(superset);
        }

    }
}