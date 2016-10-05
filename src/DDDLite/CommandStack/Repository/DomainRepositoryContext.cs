namespace DDDLite.CommandStack.Repository
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Domain;
    
    public abstract class DomainRepositoryContext : IDomainRepositoryContext
    {
        private readonly Guid id;
        private readonly ConcurrentDictionary<Type, object> cachedRepositories = new ConcurrentDictionary<Type, object>();

        protected DomainRepositoryContext()
        {
            this.id = Guid.NewGuid();
        }

        public Guid Id => this.id;

        public abstract IDomainRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        public virtual IDomainRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot
        {
            return (IDomainRepository<TAggregateRoot>)this.cachedRepositories.GetOrAdd(
                typeof(TAggregateRoot),
                this.CreateRepository<TAggregateRoot>());
        }

        public abstract void Commit();

        public abstract Task CommitAsync();

        public override string ToString() => this.id.ToString();
    }
}
