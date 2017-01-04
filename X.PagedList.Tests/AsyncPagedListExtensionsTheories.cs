using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace X.PagedList.Tests
{
    public class AsyncPagedListExtensionsTheories
    {
        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 1)]
        public async Task ToListAsync_ForQuarable_Works(int pageNumber, int pageSize, int expectedCount)
        {
            var blogs = BuildBlogList();
            var pagedBlogs = await blogs.ToPagedListAsync(pageNumber, pageSize);

            Assert.Equal(expectedCount, pagedBlogs.Count);
            Assert.Equal(pageNumber, pagedBlogs.PageNumber);
            Assert.Equal(pageSize, pagedBlogs.PageSize);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public async Task ToListAsync_ForQueryable_WithoutPageNumber_Works(int pageSize, int expectedCount)
        {
            int? pageNumber = null;
            var blogs = BuildBlogList();
            var pagedBlogs = await blogs.ToPagedListAsync(pageNumber, pageSize);

            Assert.Equal(expectedCount, pagedBlogs.Count);
            Assert.Equal(1, pagedBlogs.PageNumber);
            Assert.Equal(pageSize, pagedBlogs.PageSize);
        }

        private static IQueryable<Blog> BuildBlogList()
        {
            return new List<Blog>
            {
                new Blog() {Name = "Codding Horror", BlogID = 1, Url = "http://blog.codinghorror.com/code-smells/"},
                new Blog() {Name = "Melting Asphalt", BlogID = 2, Url = "http://www.meltingasphalt.com"},
                new Blog() {Name = "Scott Hanselman", BlogID = 3, Url = "http://www.meltingasphalt.com"}
            }.AsQueryable();
        }
    }
}