using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace X.PagedList;

/// <summary>
/// Container for async extension methods designed to simplify the creation of instances of <see cref="PagedList{T}"/>.
/// </summary>
[PublicAPI]
public static class PagedListAsyncExtensions
{
    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
    /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="totalSetCount">The total size of set</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount, CancellationToken cancellationToken)
    {
        return await Task.Factory.StartNew(() =>
        {
            return PagedListExtensions.ToPagedList(superset, pageNumber, pageSize, totalSetCount);

        }, cancellationToken);
    }
    
    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="cancellationToken"></param>
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, int? totalSetCount, CancellationToken cancellationToken)
    {
        return await ToPagedListAsync(superset.AsQueryable(), pageNumber, pageSize, totalSetCount, cancellationToken);
    }
    
    /// <summary>
    /// Async creates a subset of this collection of objects that can be individually accessed by index and
    /// containing metadata about the collection of objects the subset was created from.
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
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset, pageNumber, pageSize, totalSetCount, CancellationToken.None);
    }
    
    /// <summary>
    /// Creates a subset of this collection of objects that can be individually accessed by index and containing
    /// metadata about the collection of objects the subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <param name="superset">
    /// The collection of objects to be divided into subsets. If the collection
    /// implements <see cref="IEnumerable{T}"/>, it will be treated as such.
    /// </param>
    /// <param name="pageNumber">
    /// The one-based index of the subset of objects to be contained by this instance.
    /// </param>
    /// <param name="pageSize">The maximum size of any individual subset.</param>
    /// <param name="totalSetCount">The total size of set</param>
    /// <returns>
    /// A subset of this collection of objects that can be individually accessed by index and containing metadata
    /// about the collection of objects the subset was created from.
    /// </returns>
    /// <seealso cref="PagedList{T}"/>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, int? totalSetCount = null)
    {
        return await ToPagedListAsync(superset.AsQueryable(), pageNumber, pageSize, totalSetCount, CancellationToken.None);
    }
}
