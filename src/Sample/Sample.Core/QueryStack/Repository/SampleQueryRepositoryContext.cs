namespace Sample.Core.QueryStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.EntityFramework.QueryStack.Repository;

    using Domain;

    public class SampleQueryRepositoryContext : EFQueryRepositoryContext, ISampleQueryRepositoryContext
    {
        public SampleQueryRepositoryContext(SampleReadonlyDbContext context) : base(context)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}