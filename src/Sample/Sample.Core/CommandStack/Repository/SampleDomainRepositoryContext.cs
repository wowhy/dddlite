namespace Sample.Core.CommandStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.EntityFramework.CommandStack.Repository;

    using Domain;

    public class SampleDomainRepositoryContext : EFDomainRepositoryContext, ISampleDomainRepositoryContext
    {
        public SampleDomainRepositoryContext(SampleDomainDbContext context) : base(context)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}