using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace X.PagedList.EFCore;

/// <summary>
/// Container for extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
/// </summary>
[PublicAPI]
public static class PagedListExtensions
{
    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <returns>A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.</returns>
    /// <seealso cref="PagedList{T}"/>
    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize)
    {
        return new PagedList<T>(superset, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the
    /// collection implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>       
    /// <returns>A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.</returns>
    /// <seealso cref="PagedList{T}"/>
    public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superset)
    {
        int supersetSize = superset.Count();
        int pageSize = supersetSize == 0 ? 1 : supersetSize;
        return new PagedList<T>(superset, 1, pageSize);
    }

    /// <summary>
    /// Cast to Custom Type
    /// </summary>
    /// <param name="source">Source</param>
    /// <param name="selector">Selector</param>
    /// <typeparam name="TSource">Input Type</typeparam>
    /// <typeparam name="TResult">Result Type</typeparam>
    /// <returns>New PagedList</returns>
    public static IPagedList<TResult> Select<TSource, TResult>(this IPagedList<TSource> source, Func<TSource, TResult> selector)
    {
        var subset = ((IEnumerable<TSource>)source).Select(selector);
        return new PagedList<TResult>(source, subset);
    }

    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <typeparam name="TKey">Type For Compare</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="keySelector">Expression for Order</param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </returns>
    public static IPagedList<T> ToPagedList<T, TKey>(this IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
    {
        return new PagedList<T, TKey>(superset, keySelector, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <typeparam name="TKey">Type For Compare</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="keySelector">Expression for Order</param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>        
    public static IPagedList<T> ToPagedList<T, TKey>(this IEnumerable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
    {
        return new PagedList<T, TKey>(superset.AsQueryable(), keySelector, pageNumber, pageSize);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount, CancellationToken cancellationToken)
    {
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


        if (superset != null)
        {
            if (totalSetCount.HasValue)
            {
                totalCount = totalSetCount.Value;
            }
            else
            {
                totalCount = await superset.CountAsync(cancellationToken);
            }

            if (totalCount > 0)
            {
                subset.AddRange(
                    (pageNumber == 1)
                        ? await superset.Skip(0).Take(pageSize).ToListAsync(cancellationToken)
                        : await superset.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToListAsync(cancellationToken)
                );
            }
        }

        return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset, pageNumber, pageSize, totalSetCount, CancellationToken.None);
    }

    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, CancellationToken cancellationToken, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset, pageNumber, pageSize, totalSetCount, cancellationToken);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index (defaulting
    /// to the first page) and containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset"></param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance,  defaulting to the first page.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize, CancellationToken cancellationToken, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize, totalSetCount, cancellationToken);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index
    /// (defaulting to the first page) and containing metadata about the collection of objects the subset
    /// was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset"></param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance,
    /// defaulting to the first page.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize, totalSetCount, CancellationToken.None);
    }
}
