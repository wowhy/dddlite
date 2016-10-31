namespace DDDLite.Repository.EntityFramework
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    public class EFQueryRepositoryContext : QueryRepositoryContext, IEFQueryRepositoryContext
    {
        private readonly DbContext dbContext;

        protected DbContext DbContext => this.dbContext;

        public EFQueryRepositoryContext(DbContext context)
        {
            this.dbContext = context;
        }

        public override IQueryRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            return new EFQueryRepository<TAggregateRoot>(this);
        }

        public override IQueryable<TAggregateRoot> GetQueryModel<TAggregateRoot>()
        {
            return this.dbContext.Set<TAggregateRoot>().AsNoTracking();
        }

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters)
            where TEntity : class
        {
            return this.dbContext.Set<TEntity>().FromSql<TEntity>(sql, parameters).AsNoTracking();
        }
    }
}