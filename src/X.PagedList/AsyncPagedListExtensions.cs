using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagedList
{
    public static class AsyncPagedListExtensions
    {
        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IEnumerable{T}"/>, it will be treated as such.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset)
        {
            return Task.Factory.StartNew(superset.ToList);
        }

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <typeparam name="TKey">Type For Compare</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IEnumerable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
        {
            return Task.Factory.StartNew(() => (IPagedList<T>)(new StaticPagedList<T>(list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize), pageNumber, pageSize, list.Count())));
        }

        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <typeparam name="TKey">Type For Compare</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return AsyncPagedList<T>.CreateAsync(superset, pageNumber, pageSize);
        }
    }
}
