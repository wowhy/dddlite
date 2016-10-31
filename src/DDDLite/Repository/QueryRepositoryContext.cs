namespace DDDLite.Repository
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using Domain;

    public abstract class QueryRepositoryContext : IQueryRepositoryContext
    {
        private readonly Guid id;
        private readonly ConcurrentDictionary<Type, object> cachedRepositories = new ConcurrentDictionary<Type, object>();

        protected QueryRepositoryContext()
        {
            this.id = Guid.NewGuid();
        }

        public Guid Id => this.id;

        public abstract IQueryable<TAggregateRoot> GetQueryModel<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        public abstract IQueryRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        public virtual IQueryRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot
        {
            return (IQueryRepository<TAggregateRoot>)this.cachedRepositories.GetOrAdd(
                typeof(TAggregateRoot),
                this.CreateRepository<TAggregateRoot>());
        }

        public override string ToString() => this.id.ToString();
    }
}
