using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace X.PagedList.EF.Tests
{
    public class PagedListTheories
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        public void EFPagedList_With_IOrderedQueryable_Works(int pageNumber, int pageSize)
        {
            using (var db = new TestContext())
            {
                // arrange 
                var blogsQuery = db.Blogs;
                var orderedBlogsQuery = db.Blogs.OrderBy(b => b.BlogId);

                // act
                var blogs = new PagedList<Blog>(orderedBlogsQuery, pageNumber, pageSize);

                // assert
                Assert.Equal(pageNumber, blogs.PageNumber);
                Assert.Equal(pageSize, blogs.PageSize);
                Assert.Equal(pageNumber, blogs[0].BlogId);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        public void EFPagedList_With_KeySelector_Works(int pageNumber, int pageSize)
        {
            using (var db = new TestContext())
            {
                // arrange 
                var blogsQuery = db.Blogs;
                Expression<Func<Blog, int>> keySelector = b => b.BlogId;

                // act
                var blogs = new PagedList<Blog, int>(blogsQuery, keySelector, pageNumber, pageSize);

                // assert
                Assert.Equal(pageNumber, blogs.PageNumber);
                Assert.Equal(pageSize, blogs.PageSize);
                Assert.Equal(pageNumber, blogs[0].BlogId);
            }
        }
    }
}
