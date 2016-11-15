namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;

    using Entity;

    public abstract class SampleDbContext : DbContext
    {
        protected SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}