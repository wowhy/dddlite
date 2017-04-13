namespace Example.EF.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Core.Querying;

    public class ReadDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ReadDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Example;Trusted_Connection=True;");
        }
    }
}
