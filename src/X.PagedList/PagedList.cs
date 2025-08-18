using System;
using System.Collections.Generic;
using System.Linq;

namespace X.PagedList;

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <typeparam name="T">The type of object the collection should contain.</typeparam>
/// <seealso cref="IPagedList{T}"/>
/// <seealso cref="BasePagedList{T}"/>
/// <seealso cref="StaticPagedList{T}"/>
/// <seealso cref="List{T}"/>
public class PagedList<T> : BasePagedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset
    /// into subsets the size of the supplied pageSize. The instance then only contains the objects contained
    /// in the subset specified by index.
    /// </summary>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IQueryable<T> superset, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset.Count())
    {
        if (superset is null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        // add items to internal list
        if (TotalItemCount > 0)
        {
            var skip = (pageNumber - 1) * pageSize;

            Subset = superset.Skip(skip).Take(pageSize).ToList();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset
    /// into subsets the size of the supplied pageSize. The instance then only contains the objects contained in
    /// the subset specified by index.
    /// </summary>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IEnumerable<T> superset, int pageNumber, int pageSize)
        : this(superset.AsQueryable<T>(), pageNumber, pageSize)
    {
    }

    /// <summary>
    /// For Clone PagedList
    /// </summary>
    /// <param name="pagedList">Source PagedList</param>
    /// <param name="collection">Source collection</param>
    public PagedList(IPagedList pagedList, IEnumerable<T> collection)
    {
        TotalItemCount = pagedList.TotalItemCount;
        PageSize = pagedList.PageSize;
        PageNumber = pagedList.PageNumber;
        PageCount = pagedList.PageCount;
        HasPreviousPage = pagedList.HasPreviousPage;
        HasNextPage = pagedList.HasNextPage;
        IsFirstPage = pagedList.IsFirstPage;
        IsLastPage = pagedList.IsLastPage;
        FirstItemOnPage = pagedList.FirstItemOnPage;
        LastItemOnPage = pagedList.LastItemOnPage;

        Subset.AddRange(collection);

        if (base.Count > PageSize)
        {
            throw new Exception($"{nameof(collection)} size can't be greater than PageSize");
        }
    }

    /// <summary>
    /// Create empty static paged list
    /// </summary>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static PagedList<T> Empty(int pageSize = DefaultPageSize) =>
        new(Array.Empty<T>(), 1, pageSize);
}
