//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace X.PagedList
//{
//    /// <summary>
//    /// Used as a temprary solution. Will be updated to imporove LINQ queries for DB's
//    /// </summary>
//    public static class CollectionExtensions
//    {        
//        public static IEnumerable<T> Take<T>(this IEnumerable<T> collection, Func<int> func)
//        {
//            var take = func();
//            return collection.Take(take);
//        }

//        public static IQueryable<T> Take<T>(this IQueryable<T> collection, Func<int> func)
//        {
//            var take = func();
//            return collection.Take(take);
//        }

//        public static IEnumerable<T> Skip<T>(this IEnumerable<T> collection, Func<int> func)
//        {
//            var take = func();
//            return collection.Skip(take);
//        }

//        public static IQueryable<T> Skip<T>(this IQueryable<T> collection, Func<int> func)
//        {
//            var take = func();
//            return collection.Skip(take);
//        }
//    }
//}