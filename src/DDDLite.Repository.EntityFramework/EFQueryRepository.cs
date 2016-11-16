namespace DDDLite.Repository.EntityFramework
{
    using System.Linq;

    using DDDLite;
    using Microsoft.EntityFrameworkCore;

    public class EFQueryRepository<TAggregateRoot> : QueryRepository<TAggregateRoot>, IEFQueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly DbContext context;

        public EFQueryRepository(DbContext context)
        {
            this.context = context;
        }

        public DbContext DbContext => this.context;

        public override IQueryable<TAggregateRoot> QueryModel => this.context.Set<TAggregateRoot>();
    }
}