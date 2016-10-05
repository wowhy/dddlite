namespace Sample.Core.Common
{
    using Microsoft.EntityFrameworkCore;

    using Domain;

    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}