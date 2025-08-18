﻿using System;

namespace X.PagedList;

/// <summary>
/// Non-enumerable version of the PagedList class.
/// </summary>
[Obsolete("This class will be removed in future versions")]
public class PagedListMetaData : IPagedList
{
    /// <summary>
    /// Protected constructor that allows for instantiation without passing in a separate list.
    /// </summary>
    protected PagedListMetaData()
    {
    }

    /// <summary>
    /// Non-enumerable version of the PagedList class.
    /// </summary>
    /// <param name="pagedList">A PagedList (likely enumerable) to copy metadata from.</param>
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
}
