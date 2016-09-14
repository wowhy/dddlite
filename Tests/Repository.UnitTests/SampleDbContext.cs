using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Repository.UnitTests
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=sample;Username=postgres;Password=hongyuan;Client Encoding=UTF8;");
        }
    }

    public class Blog : AggregateRoot
    {
        public string Url { get; set; }

        public List<Post> Posts { get; set; }

        public Blog()
        {
        }

        public static Blog Create()
        {
            var blog = new Blog();
            blog.NewIdentity();
            return blog;
        }
    }

    public class Post : AggregateRoot
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
