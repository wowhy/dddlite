namespace Example.IdentityServer.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore;

    public class ExampleIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ExampleIdentityDbContext(DbContextOptions<ExampleIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.HasDefaultSchema("identity");
            base.OnModelCreating(builder);
        }
    }
}
