using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagedList
{
    public static class Extensions
    {
        /// <summary>
        /// Async create of a System.Collections.Generic.List T from an 
        /// System.Collections.Generic.IQueryable T.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">The System.Collections.Generic.IEnumerable
        /// to create a System.Collections.Generic.List from.</param>
        /// <returns> A System.Collections.Generic.List that contains elements from the input sequence.</returns>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> list)
        {
            return Task.Factory.StartNew(() => list.ToList());
        }

        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
        {
            return Task.Factory.StartNew(() => (IPagedList<T>)(new StaticPagedList<T>(list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize), pageNumber, pageSize, list.Count())));

        }
    }
}
