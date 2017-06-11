using System.Linq;
using System.Threading.Tasks;

namespace X.PagedList
{
    public class AsyncPagedList<T> : BasePagedList<T>
    {
        private AsyncPagedList(IQueryable<T> superset, int pageNumber, int pageSize)
            : base(pageNumber, pageSize, superset.Count())
        {
        }

        public static async Task<IPagedList<T>> CreateAsync(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var list = new AsyncPagedList<T>(superset, pageNumber, pageSize);


            if ((superset != null) && (list.TotalItemCount > 0))
            {
                list.Subset.AddRange(
                    (pageNumber == 1)
                        ? await superset.Skip<T>(0).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                        : await superset.Skip<T>(((pageNumber - 1) * pageSize)).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                );
            }

            return list;
        }
    }
}
