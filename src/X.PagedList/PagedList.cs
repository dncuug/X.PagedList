using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace X.PagedList;

[PublicAPI]
public class PagedList<T, TKey> : BasePagedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into
    /// subsets the size of the supplied pageSize. The instance then only contains the objects contained in the
    /// subset specified by index.
    /// </summary>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="keySelector">Expression for Order</param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset?.Count() ?? 0)
    {
        // add items to internal list
        if (TotalItemCount > 0)
        {
            InitSubset(superset, keySelector.Compile(), pageNumber, pageSize);
        }
    }

    public PagedList(IQueryable<T> superset, Func<T, TKey> keySelectorMethod, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset?.Count() ?? 0)
    {
        if (TotalItemCount > 0)
        {
            InitSubset(superset, keySelectorMethod, pageNumber, pageSize);
        }
    }

    private void InitSubset(IEnumerable<T> superset, Func<T, TKey> keySelectorMethod, int pageNumber, int pageSize)
    {
        // add items to internal list

        var items = pageNumber == 1
            ? superset.OrderBy(keySelectorMethod).Take(pageSize).ToList()
            : superset.OrderBy(keySelectorMethod).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        Subset.AddRange(items);
    }
}

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <remarks>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </remarks>
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
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IQueryable<T> superset, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset?.Count() ?? 0)
    {
        if (TotalItemCount > 0 && superset != null)
        {
            Subset.AddRange(pageNumber == 1
                ? superset.Take(pageSize).ToList()
                : superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            );
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
    /// <param name="superset">Source collection</param>
    public PagedList(IPagedList pagedList, IEnumerable<T> superset)
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

        Subset.AddRange(superset);
    }
}