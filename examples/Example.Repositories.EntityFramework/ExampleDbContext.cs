namespace Example.Repositories.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    using Example.Core.Domain;

    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(k => k.OrderLines)
                .WithOne()
                .HasForeignKey(k => k.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}