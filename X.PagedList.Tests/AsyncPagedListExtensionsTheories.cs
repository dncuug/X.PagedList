using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
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
            var mockContext = SetupMocks(BuildBlogList());

            var pagedBlogs = await mockContext.Object.Blogs.ToPagedListAsync(pageNumber, pageSize);

            Assert.Equal(expectedCount, pagedBlogs.Count);
            Assert.Equal(pageNumber, pagedBlogs.PageNumber);
            Assert.Equal(pageSize, pagedBlogs.PageSize);
        }

        private Mock<TestContext> SetupMocks(IQueryable<Blog> blogs)
        {
            var mockSet = new Mock<DbSet<Blog>>();
            mockSet.As<IDbAsyncEnumerable<Blog>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncQueryProvider<Blog>.TestDbAsyncEnumerator<Blog>(blogs.GetEnumerator()));

            mockSet.As<IQueryable<Blog>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Blog>(blogs.Provider));

            mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(blogs.Expression);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(blogs.ElementType);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(blogs.GetEnumerator());

            var mockContext = new Mock<TestContext>();
            mockContext.Setup(c => c.Blogs).Returns(mockSet.Object);

            return mockContext;
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