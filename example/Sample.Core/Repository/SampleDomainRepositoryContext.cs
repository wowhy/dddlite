namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.Repository.EntityFramework;

    using Domain;

    public class SampleDomainRepositoryContext : EFDomainRepositoryContext, ISampleDomainRepositoryContext
    {
        public SampleDomainRepositoryContext(SampleDomainDbContext context) : base(context)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}