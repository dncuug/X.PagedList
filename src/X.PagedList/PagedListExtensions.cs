using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace X.PagedList;

/// <summary>
/// Container for extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
/// </summary>
[PublicAPI]
public static class PagedListExtensions
{
    /// <summary>
    /// Splits a collection of objects into n pages with an (for example, if I have a list of 45 shoes and
    /// say 'shoes.Split(5)' I will now have 4 pages of 10 shoes and 1 page of 5 shoes.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets.</param>
    /// <param name="numberOfPages">The number of pages this collection should be split into.</param>
    /// <returns>A subset of this collection of objects, split into n pages.</returns>
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages)
    {
        int take = Convert.ToInt32(Math.Ceiling(superset.Count() / (double)numberOfPages));

        var result = new List<IEnumerable<T>>();

        for (int i = 0; i < numberOfPages; i++)
        {
            var chunk = superset.Skip(i * take).Take(take).ToList();

            if (chunk.Any())
            {
                result.Add(chunk);
            };
        }

        return result;
    }

    /// <summary>
    /// Splits a collection of objects into an unknown number of pages with n items per page (for example,
    /// if I have a list of 45 shoes and say 'shoes.Partition(10)' I will now have 4 pages of 10 shoes and
    /// 1 page of 5 shoes.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets.</param>
    /// <param name="pageSize">The maximum number of items each page may contain.</param>
    /// <returns>A subset of this collection of objects, split into pages of maximum size n.</returns>
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> superset, int pageSize)
    {
        // Cache this to avoid evaluating it twice
        var count = superset.Count();

        if (count < pageSize)
        {
            yield return superset;
        }
        else
        {
            var numberOfPages = Math.Ceiling(count / (double)pageSize);

            for (var i = 0; i < numberOfPages; i++)
            {
                yield return superset.Skip(pageSize * i).Take(pageSize);
            }
        }
    }

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
        return new PagedList<T>(superset ?? new List<T>(), pageNumber, pageSize);
    }

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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.</returns>
    /// <seealso cref="PagedList{T}"/>
    public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount)
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
                totalCount = superset.Count();
            }

            if (totalCount > 0)
            {
                subset.AddRange(
                    (pageNumber == 1)
                        ? superset.Skip(0).Take(pageSize)
                        : superset.Skip(((pageNumber - 1) * pageSize)).Take(pageSize)
                );
            }
        }

        return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
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
    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset)
    {
        var supersetSize = superset?.Count() ?? 0;
        var pageSize = supersetSize == 0 ? 1 : supersetSize;

        return new PagedList<T>(superset ?? new List<T>(), 1, pageSize);
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
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset)
    {
        return await ToListAsync(superset, CancellationToken.None);
    }

    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken)
    {
        return await Task.Run(superset.ToList, cancellationToken);
    }

    public static async Task<int> CountAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken)
    {
        return await Task.Run(superset.Count, cancellationToken);
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
                totalCount = await superset.CountAsync(cancellationToken).ConfigureAwait(false);
            }

            if (totalCount > 0)
            {
                subset.AddRange(
                    (pageNumber == 1)
                        ? await superset.Skip(0).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false)
                        : await superset.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false)
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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(AsQueryable(superset), pageNumber, pageSize, totalSetCount, CancellationToken.None);
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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, CancellationToken cancellationToken, int? totalSetCount = null)
    {
        return await ToPagedListAsync(AsQueryable(superset), pageNumber, pageSize, totalSetCount, cancellationToken);
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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(AsQueryable(superset), pageNumber, pageSize, totalSetCount, CancellationToken.None);
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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize, CancellationToken cancellationToken, int? totalSetCount = null)
    {
        return await ToPagedListAsync(AsQueryable(superset), pageNumber ?? 1, pageSize, totalSetCount, cancellationToken);
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
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(AsQueryable(superset), pageNumber ?? 1, pageSize, totalSetCount, CancellationToken.None);
    }

    private static IQueryable<T> AsQueryable<T>(IQueryable<T> superset)
    {
        return superset ?? new EnumerableQuery<T>(new List<T>());
    }
    
    private static IQueryable<T> AsQueryable<T>(IEnumerable<T> superset)
    {
        return superset == null ? new EnumerableQuery<T>(new List<T>()) : superset.AsQueryable();
    }
}
