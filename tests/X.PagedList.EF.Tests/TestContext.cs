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

    class TestContextDBInitializer : DropCreateDatabaseAlways<TestContext>
    {
        protected override void Seed(TestContext context)
        {
            // Create a couple of blog entries.
            IList<Blog> defaultBlogs = new List<Blog>();
            for (int i = 1; i <= 10; i++)
            {
                defaultBlogs.Add(new Blog() {BlogId = i, Name = $"Blog {i}", Url = $"http://blogs.com/{i}"});
            }

            context.Blogs.AddRange(defaultBlogs);

            base.Seed(context);
        }
    }

    class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set;  }
    }
}
