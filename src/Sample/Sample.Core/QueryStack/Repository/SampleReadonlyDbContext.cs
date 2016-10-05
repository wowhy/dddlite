namespace Sample.Core.QueryStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using Common;

    public class SampleReadonlyDbContext : SampleDbContext
    {
        public SampleReadonlyDbContext(DbContextOptions<SampleReadonlyDbContext> options) : base(options)
        {
        }
    }
}