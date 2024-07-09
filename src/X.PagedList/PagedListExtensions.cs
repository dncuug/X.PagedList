using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

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
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        // Cache this to avoid evaluating it twice
        int count = superset.Count();

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
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the
    /// collection implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <returns>A subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.</returns>
    /// <seealso cref="PagedList{T}"/>
    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset)
    {
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        int supersetSize = superset.Count();
        int pageSize = supersetSize == 0 ? 1 : supersetSize;

        return new PagedList<T>(superset, 1, pageSize);
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
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        return new PagedList<T>(superset, pageNumber, pageSize);
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
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        return new PagedList<T, TKey>(superset.AsQueryable(), keySelector, pageNumber, pageSize);
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

        int totalCount = totalSetCount ?? superset.Count();

        List<T> subset;

        if (totalCount > 0)
        {
            var skip = (pageNumber - 1) * pageSize;

            subset = superset.Skip(skip).Take(pageSize).ToList();
        }
        else
        {
            subset = new List<T>();
        }

        return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
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
        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        return new PagedList<T, TKey>(superset, keySelector, pageNumber, pageSize);
    }
}
