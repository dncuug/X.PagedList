using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace X.PagedList.EF
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <remarks>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </remarks>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IPagedList{T}"/>
    /// <seealso cref="BasePagedList{T}"/>
    /// <seealso cref="StaticPagedList{T}"/>
    /// <seealso cref="List{T}"/>    
    public class PagedList<T> : BasePagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IOrderedQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IOrderedQueryable<T> superset, int pageNumber, int pageSize)
            : base(pageNumber, pageSize, superset?.Count() ?? 0)
        {
            if (TotalItemCount > 0)
            {
                InitSubset(superset, pageNumber, pageSize);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable"/>, it will be treated as such.</param>
        /// <param name="keySelector">Expression for Order</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IQueryable<T> superset, Expression<Func<T, object>> keySelector, int pageNumber, int pageSize)
            : base(pageNumber, pageSize, superset?.Count() ?? 0)
        {
            // add items to internal list
            if (TotalItemCount > 0)
            {
                InitSubset(superset, keySelector, pageNumber, pageSize);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IOrderedEnumerable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IOrderedEnumerable<T> superset, int pageNumber, int pageSize)
            : this(superset.AsQueryable() as IOrderedQueryable<T>, pageNumber, pageSize)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IEnumerable"/>, it will be treated as such.</param>
        /// <param name="keySelector">Expression for Order</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IEnumerable<T> superset, Expression<Func<T, object>> keySelector, int pageNumber, int pageSize)
            : this(superset.AsQueryable(), keySelector, pageNumber, pageSize)
        {
        }

        /// <summary>
        /// For Clone PagedList
        /// </summary>
        /// <param name="pagedList">Source PagedList</param>
        /// <param name="superset">Source collection</param>
        public PagedList(IPagedList pagedList, IEnumerable<T> superset)
        {
            TotalItemCount = pagedList.TotalItemCount;
            PageSize = pagedList.PageSize;
            PageNumber = pagedList.PageNumber;
            PageCount = pagedList.PageCount;
            HasPreviousPage = pagedList.HasPreviousPage;
            HasNextPage = pagedList.HasNextPage;
            IsFirstPage = pagedList.IsFirstPage;
            IsLastPage = pagedList.IsLastPage;
            FirstItemOnPage = pagedList.FirstItemOnPage;
            LastItemOnPage = pagedList.LastItemOnPage;

            Subset.AddRange(superset);
        }

        private void InitSubset(IOrderedQueryable<T> superset, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;

            // add items to internal list
            var items = superset
                .Skip(() => itemsToSkip)
                .Take(() => pageSize)
                .ToList();

            Subset.AddRange(items);
        }

        private void InitSubset(IQueryable<T> superset, Expression<Func<T, object>> keySelector, int pageNumber, int pageSize)
        {
            MemberExpression propertyExpression;

            switch (keySelector.Body)
            {
                case MemberExpression memberExpression:
                    propertyExpression = memberExpression;
                    break;
                case UnaryExpression unaryExpression:
                    propertyExpression = unaryExpression.Operand as MemberExpression;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported expression.");
            }

            var sourceType = typeof(T);
            var orderExpression = Expression.Lambda(propertyExpression, keySelector.Parameters[0]);
            var resultExpression = Expression.Call(typeof(Queryable),
                "OrderBy",
                new[] { sourceType, propertyExpression.Type },
                superset.Expression,
                Expression.Quote(orderExpression));

            var orderedSuperset = superset.Provider.CreateQuery<T>(resultExpression) as IOrderedQueryable<T>;

            InitSubset(orderedSuperset, pageNumber, pageSize);
        }
    }
}
