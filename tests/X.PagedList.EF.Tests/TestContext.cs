using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace X.PagedList.EF.Tests
{
    class TestContext: DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public TestContext()
        {
            Database.SetInitializer(new TestContextDBInitializer());
        }
    }
}
