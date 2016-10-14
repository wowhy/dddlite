namespace DDDLite.Repository.EntityFramework
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using DDDLite.Domain;

    public class EFQueryRepositoryContext : QueryRepositoryContext, IEFQueryRepositoryContext
    {
        private readonly DbContext dbContext;

        public EFQueryRepositoryContext(DbContext context)
        {
            this.dbContext = context;
        }

        public DbContext DbContext => this.dbContext;

        public override IQueryRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            return new EFQueryRepository<TAggregateRoot>(this);
        }

        public override IQueryable<TAggregateRoot> GetQueryableModel<TAggregateRoot>()
        {
            return this.dbContext.Set<TAggregateRoot>();
        }
    }
}