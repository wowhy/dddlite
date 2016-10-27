namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;
    
    public class SampleDomainDbContext : SampleDbContext
    {
        public SampleDomainDbContext(DbContextOptions<SampleDomainDbContext> options) : base(options)
        {
        }
    }
}