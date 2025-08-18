using System;
using System.Collections;
using System.Collections.Generic;

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
public abstract class BasePagedList<T> : IPagedList<T>
{
    protected List<T> Subset = new();

    public const int DefaultPageSize = 100;

    /// <summary>
    /// Total number of subsets within the superset.
    /// </summary>
    public int PageCount { get; protected set; }

    /// <summary>
    /// Total number of objects contained within the superset.
    /// </summary>
    public int TotalItemCount { get; protected set; }

    /// <summary>
    /// One-based index of this subset within the superset, zero if the superset is empty.
    /// </summary>
    public int PageNumber { get; protected set; }

    /// <summary>
    /// Maximum size any individual subset.
    /// </summary>
    public int PageSize { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the first subset within the superset.
    /// </summary>
    public bool HasPreviousPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the last subset within the superset.
    /// </summary>
    public bool HasNextPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this  is
    /// the first subset within the superset.
    /// </summary>
    public bool IsFirstPage { get; protected set; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is
    /// the last subset within the superset.
    /// </summary>
    public bool IsLastPage { get; protected set; }

    /// <summary>
    /// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </summary>
    public int FirstItemOnPage { get; protected set; }

    /// <summary>
    /// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber is
    /// greater than PageCount.
    /// </summary>
    public int LastItemOnPage { get; protected set; }

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
            throw new ArgumentOutOfRangeException(
                $"totalItemCount = {totalItemCount}. TotalItemCount cannot be less than 0.");
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
            ? numberOfLastItemOnPage > TotalItemCount ? TotalItemCount : numberOfLastItemOnPage
            : 0;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
    /// </summary>
    /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return Subset.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
    /// </summary>
    /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name = "index">The zero-based index of the element to get.</param>
    public T this[int index] => Subset[index];

    /// <summary>
    /// Gets the number of elements contained on this page.
    /// </summary>
    public virtual int Count => Subset.Count;

    /// <summary>
    /// Gets a non-enumerable copy of this paged list.
    /// </summary>
    /// <returns>A non-enumerable copy of this paged list.</returns>
    public PagedListMetaData GetMetaData() => new(this);
}