namespace DDDLite.Repository.EntityFramework
{
    using System.Linq;

    using DDDLite;
    using Microsoft.EntityFrameworkCore;

    public class EFQueryRepository<TAggregateRoot> : QueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private DbContext context;

        public EFQueryRepository(DbContext context)
        {
            this.context = context;
        }

        public override IQueryable<TAggregateRoot> QueryModel => this.context.Set<TAggregateRoot>();
    }
}