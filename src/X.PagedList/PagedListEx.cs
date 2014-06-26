using System;
using System.Collections.Generic;
using System.Linq;

namespace PagedList
{
    public class PagedListEx<T> : BasePagedList<T>
    {
        private PagedListEx()
        { 
        }

        public static async Task<IPagedList<T>> Create(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var list = new PagedListEx<T>();
            await list.Init(superset, pageNumber, pageSize);
            return list;
        }

        async Task Init(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
            }
            base.TotalItemCount = (superset == null) ? 0 : superset.Count<T>();
            base.PageSize = pageSize;
            base.PageNumber = pageNumber;
            base.PageCount = (base.TotalItemCount > 0) ? ((int)Math.Ceiling((double)(((double)base.TotalItemCount) / ((double)base.PageSize)))) : 0;
            base.HasPreviousPage = base.PageNumber > 1;
            base.HasNextPage = base.PageNumber < base.PageCount;
            base.IsFirstPage = base.PageNumber == 1;
            base.IsLastPage = base.PageNumber >= base.PageCount;
            base.FirstItemOnPage = ((base.PageNumber - 1) * base.PageSize) + 1;
            int num = (base.FirstItemOnPage + base.PageSize) - 1;
            base.LastItemOnPage = (num > base.TotalItemCount) ? base.TotalItemCount : num;
            if ((superset != null) && (base.TotalItemCount > 0))
            {
                base.Subset.AddRange(
                    (pageNumber == 1) ?
                    await superset.Skip<T>(0).Take<T>(pageSize).ToListAsync<T>() : 
                    await superset.Skip<T>(((pageNumber - 1) * pageSize)).Take<T>(pageSize).ToListAsync<T>()
                    );
            }
        }
    }
}
