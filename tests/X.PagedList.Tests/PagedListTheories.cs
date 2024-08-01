using AutoFixture;
using System.Linq;
using Xunit;

namespace X.PagedList.Tests;

public class PagedListTheories
{
    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(2, 2, 1)]
    public void ToList_ForQuarable_Works(int pageNumber, int pageSize, int expectedCount)
    {
        var blogs = BuildBlogList();
        var pagedBlogs = blogs.ToPagedList(pageNumber, pageSize);

        Assert.Equal(expectedCount, pagedBlogs.Count);
        Assert.Equal(pageNumber, pagedBlogs.PageNumber);
        Assert.Equal(pageSize, pagedBlogs.PageSize);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    public void ToList_ForQueryable_WithoutPageNumber_Works(int pageSize, int expectedCount)
    {
        int? pageNumber = null;
        var blogs = BuildBlogList();
        var pagedBlogs = blogs.ToPagedList(pageNumber ?? 1, pageSize);

        Assert.Equal(expectedCount, pagedBlogs.Count);
        Assert.Equal(1, pagedBlogs.PageNumber);
        Assert.Equal(pageSize, pagedBlogs.PageSize);
    }

    [Theory]
    [InlineData(1000, 100, 10, null)]
    [InlineData(1000, 200, 5, 3)]
    [InlineData(1000, 300, 4, 4)]
    public void ToList_ForQueryable_With_TotalSetCount_Works(int superSetTotalCount, int pageSize, int expectedCount, int? pageNumber = null)
    {
        pageNumber = pageNumber.HasValue == false ? 0 : pageNumber;

        int listPageNumber = pageNumber != 0 ? (pageNumber ?? 1) - 1 : (pageNumber ?? 1);
        int xListPageNumber = pageNumber == 0 ? 1 : (pageNumber ?? 1);

        var superset = BuildBlogList(1000);

        var pageOfSuperSet = superset.Skip(listPageNumber * pageSize).Take(pageSize).ToList();
        var pagedBlogs = superset.AsQueryable().ToPagedList(xListPageNumber, pageSize, superSetTotalCount);
        var pagedBlogsWithoutTotalCount = superset.AsQueryable().ToPagedList(xListPageNumber, pageSize);

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


    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 1, 1, false, true)]
    [InlineData(new[] { 1, 2, 3 }, 2, 1, true, true)]
    [InlineData(new[] { 1, 2, 3 }, 3, 1, true, false)]
    [InlineData(new[] { 1, 2, 3 }, 1, 3, false, false)]
    [InlineData(new[] { 1, 2, 3 }, 2, 3, false, false)]
    [InlineData(new int[] { }, 1, 3, false, false)]
    public void Theory_HasPreviousPage_And_HasNextPage_Are_Correct(int[] integers, int pageNumber, int pageSize,
        bool expectedHasPrevious, bool expectedHasNext)
    {
        //arrange
        int[] data = integers;

        //act
        var pagedList = data.ToPagedList(pageNumber, pageSize);

        //assert
        Assert.Equal(expectedHasPrevious, pagedList.HasPreviousPage);
        Assert.Equal(expectedHasNext, pagedList.HasNextPage);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 1, 1, true, false)]
    [InlineData(new[] { 1, 2, 3 }, 2, 1, false, false)]
    [InlineData(new[] { 1, 2, 3 }, 3, 1, false, true)]
    [InlineData(new[] { 1, 2, 3 }, 1, 3, true, true)] // Page 1 of 1
    [InlineData(new[] { 1, 2, 3 }, 2, 3, false, false)] // Page 2 of 1
    [InlineData(new int[] { }, 1, 3, false, false)] // Page 1 of 0
    public void Theory_IsFirstPage_And_IsLastPage_Are_Correct(int[] integers, int pageNumber, int pageSize,
        bool expectedIsFirstPage, bool expectedIsLastPage)
    {
        //arrange
        int[] data = integers;

        //act
        var pagedList = data.ToPagedList(pageNumber, pageSize);

        //assert
        Assert.Equal(expectedIsFirstPage, pagedList.IsFirstPage);
        Assert.Equal(expectedIsLastPage, pagedList.IsLastPage);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 1, 3)]
    [InlineData(new[] { 1, 2, 3 }, 3, 1)]
    [InlineData(new[] { 1 }, 1, 1)]
    [InlineData(new[] { 1, 2, 3 }, 2, 2)]
    [InlineData(new[] { 1, 2, 3, 4 }, 2, 2)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 2, 3)]
    [InlineData(new int[] { }, 1, 0)]
    public void Theory_PageCount_Is_Correct(int[] integers, int pageSize, int expectedNumberOfPages)
    {
        //arrange
        int[] data = integers;

        //act
        var pagedList = data.ToPagedList(1, pageSize);

        //assert
        Assert.Equal(expectedNumberOfPages, pagedList.PageCount);
    }


    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 1, 2, 1, 2)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 2, 2, 3, 4)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 3, 2, 5, 5)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 4, 2, 0, 0)]
    [InlineData(new int[] { }, 1, 2, 0, 0)]
    public void Theory_FirstItemOnPage_And_LastItemOnPage_Are_Correct(int[] integers, int pageNumber, int pageSize, int expectedFirstItemOnPage, int expectedLastItemOnPage)
    {
        //arrange
        int[] data = integers;

        //act
        var pagedList = data.ToPagedList(pageNumber, pageSize);

        //assert
        Assert.Equal(expectedFirstItemOnPage, pagedList.FirstItemOnPage);
        Assert.Equal(expectedLastItemOnPage, pagedList.LastItemOnPage);
    }

    private static IQueryable<Blog> BuildBlogList(int itemCount = 3)
    {
        var fixture = new Fixture();

        return fixture.CreateMany<Blog>(itemCount).OrderByDescending(b => b.BlogID).AsQueryable();
    }
}
