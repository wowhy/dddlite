namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;

    public class SampleReadonlyDbContext : SampleDbContext
    {
        public SampleReadonlyDbContext(DbContextOptions<SampleReadonlyDbContext> options) : base(options)
        {
        }
    }
}