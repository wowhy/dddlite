namespace TestWebApi.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Aggregates;

    public class DemoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DemoDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Demo;Trusted_Connection=True;");
            optionsBuilder.UseSqlite("Filename=./Demo.db");
        }
    }
}
