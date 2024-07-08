namespace X.PagedList.Tests;

public class TestContext : DbContext
{
    public virtual DbSet<Blog> Blogs { get; set; }
}
