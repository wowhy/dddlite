namespace Example.WebApi.Data
{
    using DDDLite.WebApi.Data;

    using Microsoft.EntityFrameworkCore;

    public class ExampleIdentityDbContext : ApplicationDbContext
    {
        public ExampleIdentityDbContext(DbContextOptions<ExampleIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("identity");
            base.OnModelCreating(builder);
        }
    }
}
