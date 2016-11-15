namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;

    public class SampleMasterDbContext : SampleDbContext
    {
        public SampleMasterDbContext(DbContextOptions<SampleMasterDbContext> options) : base(options)
        {
        }
    }
}