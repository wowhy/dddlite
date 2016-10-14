namespace Sample.Core.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.Repository.EntityFramework;

    using Domain;

    public class SampleQueryRepositoryContext : EFQueryRepositoryContext, ISampleQueryRepositoryContext
    {
        public SampleQueryRepositoryContext(SampleReadonlyDbContext context) : base(context)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}