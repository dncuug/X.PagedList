using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace X.PagedList.Tests;

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
    
    [Theory]
    [InlineData(1000, 100, 10, null)]
    [InlineData(1000, 200, 5, 3)]
    [InlineData(1000, 300, 4, 4)]
    public async Task ToListAsync_ForQueryable_With_TotalSetCount_Works(int superSetTotalCount, int pageSize, int expectedCount, int? pageNumber = null)
    {
        
        pageNumber = pageNumber.HasValue == false ? 0 : pageNumber;
        var listPageNumber = pageNumber != 0 ? pageNumber - 1 : pageNumber;
        var xListPageNumber = pageNumber == 0 ? 1 : pageNumber;
        
        var superset = BuildBlogList(1000);
        
        var pageOfSuperSet = superset.Skip(listPageNumber.Value * pageSize).Take(pageSize).ToList();
        var pagedBlogs = await pageOfSuperSet.AsQueryable().ToPagedListAsync(xListPageNumber, pageSize, superSetTotalCount);
        var pagedBlogsWithoutTotalCount = await superset.AsQueryable().ToPagedListAsync(xListPageNumber, pageSize);
        
        //test the totalSetCount extension
        Assert.Equal(expectedCount, pagedBlogs.PageCount);
        Assert.Equal(xListPageNumber, pagedBlogs.PageNumber);
        Assert.Equal(pageSize, pagedBlogs.PageSize);
        Assert.Equal(pageOfSuperSet.Count(), pagedBlogs.Count);
        Assert.Equal(pageOfSuperSet.First().Name, pagedBlogs.OrderByDescending(b => b.BlogID).First().Name);
        
        //test the default behaviour
        Assert.Equal(expectedCount, pagedBlogsWithoutTotalCount.PageCount);
        Assert.Equal(xListPageNumber, pagedBlogsWithoutTotalCount.PageNumber);
        Assert.Equal(pageSize, pagedBlogsWithoutTotalCount.PageSize);
        Assert.Equal(pageOfSuperSet.Count(), pagedBlogsWithoutTotalCount.Count);
        Assert.Equal(pageOfSuperSet.First().Name, pagedBlogsWithoutTotalCount.OrderByDescending(b => b.BlogID).First().Name);
    }

    [Fact]
    public async Task ToListAsync_Check_CornerCases()
    {
        var pageNumber = 2;
        var pageSize = 10;
        var superSetTotalCount = 110;

        var superset = BuildBlogList(50);
        var queryable = superset.AsQueryable();
        
        var pagedList = await queryable.ToPagedListAsync(pageNumber, pageSize, superSetTotalCount);
        var pagedListWithoutTotalCount = await queryable.ToPagedListAsync(pageNumber, pageSize);

        //test the totalSetCount extension
        Assert.Equal(11, pagedList.PageCount);
        Assert.Equal(2, pagedList.PageNumber);
        Assert.Equal(pageSize, pagedList.PageSize);
        Assert.Equal(10, pagedList.Count);
        
        //test the pagedListWithoutTotalCount extension
        Assert.Equal(11, pagedList.PageCount);
        Assert.Equal(2, pagedListWithoutTotalCount.PageNumber);
        Assert.Equal(pageSize, pagedListWithoutTotalCount.PageSize);
        Assert.Equal(10, pagedListWithoutTotalCount.Count);
    }

    private static IQueryable<Blog> BuildBlogList(int itemCount = 3)
    {
        var fixture = new Fixture();
        
        return fixture.CreateMany<Blog>(itemCount).OrderByDescending(b => b.BlogID).AsQueryable();
    }
}