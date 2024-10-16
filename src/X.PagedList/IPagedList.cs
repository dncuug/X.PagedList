using System;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace X.PagedList;

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <typeparam name="T">The type of object the collection should contain.</typeparam>
/// <seealso cref="IReadOnlyList{T}"/>
[PublicAPI]
public interface IPagedList<out T> : IPagedList, IReadOnlyList<T>
{
    /// <summary>
    /// Gets a non-enumerable copy of this paged list.
    /// </summary>
    /// <returns>A non-enumerable copy of this paged list.</returns>
    [Obsolete("This method will be removed in future versions")]
    PagedListMetaData GetMetaData();
}

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </summary>
public interface IPagedList
{
    /// <summary>
    /// Total number of subsets within the superset.
    /// </summary>
    int PageCount { get; }

    /// <summary>
    /// Total number of objects contained within the superset.
    /// </summary>
    int TotalItemCount { get; }

    /// <summary>
    /// One-based index of this subset within the superset, zero if the superset is empty.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Maximum size any individual subset.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the first subset within the superset.
    /// </summary>
    bool HasPreviousPage { get; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is NOT the last subset within the superset.
    /// </summary>
    bool HasNextPage { get; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is the first subset within the superset.
    /// </summary>
    bool IsFirstPage { get; }

    /// <summary>
    /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
    /// is the last subset within the superset.
    /// </summary>
    bool IsLastPage { get; }

    /// <summary>
    /// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </summary>
    int FirstItemOnPage { get; }

    /// <summary>
    /// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber
    /// is greater than PageCount.
    /// </summary>
    int LastItemOnPage { get; }
}
