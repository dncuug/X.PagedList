using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PagedList
{
    /// <summary>
    /// Container for extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize)
        {
            return new PagedList<T>(superset, pageNumber, pageSize);
        }
             

        /// <summary>
        /// Splits a collection of objects into n pages with an (for example, if I have a list of 45 shoes and say 'shoes.Split(5)' I will now have 4 pages of 10 shoes and 1 page of 5 shoes.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets.</param>
        /// <param name="numberOfPages">The number of pages this collection should be split into.</param>
        /// <returns>A subset of this collection of objects, split into n pages.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages)
        {
            return superset
                .Select((item, index) => new { index, item })
                .GroupBy(x => x.index % numberOfPages)
                .Select(x => x.Select(y => y.item));
        }

        /// <summary>
        /// Splits a collection of objects into an unknown number of pages with n items per page (for example, if I have a list of 45 shoes and say 'shoes.Partition(10)' I will now have 4 pages of 10 shoes and 1 page of 5 shoes.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets.</param>
        /// <param name="pageSize">The maximum number of items each page may contain.</param>
        /// <returns>A subset of this collection of objects, split into pages of maximum size n.</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> superset, int pageSize)
        {
            // Cache this to avoid evaluating it twice
            int count = superset.Count();
            if (count < pageSize)
                yield return superset;
            else
            {
                var numberOfPages = Math.Ceiling(count / (double)pageSize);
                for (var i = 0; i < numberOfPages; i++)
                    yield return superset.Skip(pageSize * i).Take(pageSize);
            }
        }

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <typeparam name="TKey">Type For Compare</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static IPagedList<T> ToPagedList<T, TKey>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return new PagedList<T>(superset, pageNumber, pageSize);
        }
           
       
        /// <summary>
        /// Cast to Custom Type
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="selector">Selector</param>
        /// <typeparam name="TSource">Input Type</typeparam>
        /// <typeparam name="TResult">Result Type</typeparam>
        /// <returns>New PagedList</returns>
        public static IPagedList<TResult> Select<TSource, TResult>(this PagedList<TSource> source, Func<TSource, TResult> selector)
        {
            var subset = ((IEnumerable<TSource>)source).Select(selector);
            return new PagedList<TResult>(source, subset);
        }

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <typeparam name="TKey">Type For Compare</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="keySelector">Expression for Order</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedListForEntityFramework{T, TKey}"/>
        public static IPagedList<T> ToPagedListForEntityFramework<T, TKey>(this IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
        {
            return new PagedListForEntityFramework<T, TKey>(superset, keySelector, pageNumber, pageSize);

        }

    }

}