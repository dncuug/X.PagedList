using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace X.PagedList;

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <remarks>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </remarks>
/// <typeparam name = "T">The type of object the collection should contain.</typeparam>
/// <seealso cref = "IPagedList{T}" />
/// <seealso cref = "List{T}" />
[PublicAPI]
public abstract class BasePagedList<T> : IPagedList<T>
{
    private List<T> _subset = new();

    public const int DefaultPageSize = 100;

    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    protected internal BasePagedList()
    {
    }

    /// <summary>
    /// Initializes a new instance of a type deriving from <see cref = "BasePagedList{T}" /> and sets properties
    /// needed to calculate position and size data on the subset and superset.
    /// </summary>
    /// <param name = "pageNumber">The one-based index of the subset of objects contained by this instance.</param>
    /// <param name = "pageSize">The maximum size of any individual subset.</param>
    /// <param name = "totalItemCount">The size of the superset.</param>
    protected internal BasePagedList(int pageNumber, int pageSize, int totalItemCount)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
        }

        if (totalItemCount < 0)
        {
            throw new ArgumentOutOfRangeException($"totalItemCount = {totalItemCount}. TotalItemCount cannot be less than 0.");
        }

        // set source to blank list if superset is null to prevent exceptions
        TotalItemCount = totalItemCount;

        PageSize = pageSize;
        PageNumber = pageNumber;
        PageCount = TotalItemCount > 0
            ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
            : 0;

        bool pageNumberIsGood = PageCount > 0 && PageNumber <= PageCount;

        HasPreviousPage = pageNumberIsGood && PageNumber > 1;
        HasNextPage = pageNumberIsGood && PageNumber < PageCount;
        IsFirstPage = pageNumberIsGood && PageNumber == 1;
        IsLastPage = pageNumberIsGood && PageNumber == PageCount;

        int numberOfFirstItemOnPage = (PageNumber - 1) * PageSize + 1;

        FirstItemOnPage = pageNumberIsGood ? numberOfFirstItemOnPage : 0;

        int numberOfLastItemOnPage = numberOfFirstItemOnPage + PageSize - 1;

        LastItemOnPage = pageNumberIsGood
            ? numberOfLastItemOnPage > TotalItemCount
                ? TotalItemCount
                : numberOfLastItemOnPage
            : 0;
    }

    protected void SetSubset(IEnumerable<T> subset)
    {
        _subset.Clear();
        _subset.AddRange(subset);
        
    }

    /// <summary>
    /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
    /// </summary>
    /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
    // [MustDisposeResource]
    public IEnumerator<T> GetEnumerator()
    {
        return _subset.GetEnumerator();
    }

    /// <summary>
    /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
    /// </summary>
    /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///	    Gets the element at the specified index.
    /// </summary>
    /// <param name = "index">The zero-based index of the element to get.</param>
    public T this[int index] => Subset[index];

    /// <summary>
    /// 	Gets the number of elements contained on this page.
    /// </summary>
    public virtual int Count => _subset.Count;

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
    ///     One-based index of this subset within the superset, zero if the superset is empty.
    /// </summary>
    /// <value>
    /// 	One-based index of this subset within the superset, zero if the superset is empty.
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
    /// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
    /// </summary>
    /// <value>
    /// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
    /// </value>
    public bool HasPreviousPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the last subset within the superset.
    /// </summary>
    /// <value>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the last subset within the superset.
    /// </value>
    public bool HasNextPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is the first subset within the superset.
    /// </summary>
    /// <value>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and
    /// this is the first subset within the superset.
    /// </value>
    public bool IsFirstPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and
    /// this is the last subset within the superset.
    /// </summary>
    /// <value>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is the last subset within the superset.
    /// </value>
    public bool IsLastPage { get; protected set; }

    /// <summary>
    /// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </summary>
    /// <value>
    /// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </value>
    public int FirstItemOnPage { get; protected set; }

    /// <summary>
    /// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </summary>
    /// <value>
    /// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </value>
    public int LastItemOnPage { get; protected set; }

    /// <summary>
    /// Return clean metadata without collection inside
    /// </summary>
    /// <returns></returns>
    [Obsolete("This method will be removed in the next major version.")]
    public IPagedList GetMetaData()
    {
        return new StaticPagedList<object>(ImmutableArray<object>.Empty, this);
    }
}