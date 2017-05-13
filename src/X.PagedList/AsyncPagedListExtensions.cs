using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace X.PagedList
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
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset)
        {
            return await Task.Run(() => superset.ToList());
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
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var count = list.Count();

            return await Task.Run(() => (IPagedList<T>)(new StaticPagedList<T>(list.Skip(skip).Take(pageSize), pageNumber, pageSize, count)));
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
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return await AsyncPagedList<T>.CreateAsync(superset, pageNumber, pageSize);
        }

        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index (defaulting to the first page) and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <typeparam name="TKey">Type For Compare</typeparam>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance, defaulting to the first page.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize)
        {
            return await superset.ToPagedListAsync(pageNumber ?? 1, pageSize);
        }
    }
}
