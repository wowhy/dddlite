namespace Sample.Core.CommandStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using Common;
    
    public class SampleDomainDbContext : SampleDbContext
    {
        public SampleDomainDbContext(DbContextOptions<SampleDomainDbContext> options) : base(options)
        {
        }
    }
}