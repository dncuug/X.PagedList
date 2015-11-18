using System;
using System.Linq;
using System.Threading.Tasks;

namespace PagedList
{
    public class AsyncPagedList<T> : BasePagedList<T>
    {
        private AsyncPagedList()
        {
        }

        public static async Task<IPagedList<T>> CreateAsync(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var list = new AsyncPagedList<T>();
            await list.InitAsync(superset, pageNumber, pageSize).ConfigureAwait(false);
            return list;
        }

        private async Task InitAsync(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber: " + pageNumber, "PageNumber cannot be below 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize: " + pageSize, "PageSize cannot be less than 1.");
            }

            TotalItemCount = (superset == null) ? 0 : superset.Count<T>();
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = (TotalItemCount > 0) ? ((int)Math.Ceiling(((double)TotalItemCount) / PageSize)) : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = ((PageNumber - 1) * PageSize) + 1;
            var num = (FirstItemOnPage + PageSize) - 1;
            LastItemOnPage = (num > TotalItemCount) ? TotalItemCount : num;

            if ((superset != null) && (TotalItemCount > 0))
            {
                Subset.AddRange(
                    (pageNumber == 1)
                        ? await superset.Skip<T>(0).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                        : await superset.Skip<T>(((pageNumber - 1) * pageSize)).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                    );
            }
        }
    }
}
