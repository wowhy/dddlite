namespace Example.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Core.Domain;
    using Microsoft.EntityFrameworkCore.Metadata;

    public class WriteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public WriteDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Example;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(k => k.EnabledBy)
                .WithMany()
                .HasForeignKey(k => k.EnabledById)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
