using System;

namespace PagedList
{
    /// <summary>
    /// Paging settings for 3-rd part tools
    /// </summary>
    public static class Paging
    {
        static Paging()
        {
            PageSize = 15;
        }

        /// <summary>
        /// Default page size
        /// </summary>
        public static int PageSize { get; set; }
    }
}
