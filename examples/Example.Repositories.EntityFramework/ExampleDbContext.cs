namespace Example.Repositories.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.Repositories.EntityFramework;

    using Example.Core.Domain;
    using Example.Repositories.EntityFramework.Configurations;

    public class ExampleDbContext : UnitOfWorkDbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}