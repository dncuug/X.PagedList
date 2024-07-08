using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace X.PagedList.EF;

/// <summary>
/// EntityFramework extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
/// </summary>
[PublicAPI]
public static class PagedListExtensions
{
    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="totalSetCount">The total size of set</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T>? superset, int pageNumber, int pageSize, int? totalSetCount, CancellationToken cancellationToken)
    {
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
        }

        var subset = new List<T>();
        var totalCount = 0;

        if (totalSetCount.HasValue)
        {
            totalCount = totalSetCount.Value;
        }
        else
        {
            totalCount = await superset.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        if (totalCount > 0)
        {
            var skip = (pageNumber - 1) * pageSize;

            subset.AddRange(await superset.Skip(skip).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false));
        }

        return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return ToPagedListAsync(superset, pageNumber, pageSize, totalSetCount, CancellationToken.None);
    }
}
