using System;
using System.Threading.Tasks;

namespace DDDLite.Repositories
{
    public abstract class DomainRepository<TAggregateRoot> : IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected DomainRepository()
        {
        }

        public abstract Task<TAggregateRoot> GetByIdAsync(Guid key);

        public abstract Task SaveAsync(TAggregateRoot aggregateRoot);

        public abstract Task DeleteAsync(TAggregateRoot aggregateRoot);
    }
}
