using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace X.PagedList;

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// This implementation support ordering by key.
/// </summary>
/// <remarks>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing
/// metadata about the superset collection of objects this subset was created from.
/// </remarks>
/// <typeparam name="T">The type of object the collection should contain.</typeparam>
/// <typeparam name="TKey"></typeparam>
[PublicAPI]
public class PagedList<T, TKey> : BasePagedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into
    /// subsets the size of the supplied pageSize. The instance then only contains the objects contained in the
    /// subset specified by index.
    /// </summary>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IQueryable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="keySelectorExpression">Expression for OrderBy</param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IQueryable<T> superset, Expression<Func<T, TKey>> keySelectorExpression, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset.Count())
    {
        if (superset is null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        // add items to internal list
        if (TotalItemCount > 0)
        {
            var skip = (pageNumber - 1) * pageSize;

            Subset = superset.OrderBy(keySelectorExpression).Skip(skip).Take(pageSize).ToList();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into
    /// subsets the size of the supplied pageSize. The instance then only contains the objects contained in the
    /// subset specified by index.
    /// </summary>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="keySelectorMethod">Function delegate for OrderBy</param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
    public PagedList(IEnumerable<T> superset, Func<T, TKey> keySelectorMethod, int pageNumber, int pageSize)
        : base(pageNumber, pageSize, superset.Count())
    {
        if (superset is null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        // add items to internal list
        if (TotalItemCount > 0)
        {
            var skip = (pageNumber - 1) * pageSize;

            Subset = superset.OrderBy(keySelectorMethod).Skip(skip).Take(pageSize).ToList();
        }
    }
}
