namespace DDDLite.Repositories
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        private readonly Guid id;
        private readonly ConcurrentDictionary<Type, object> cacheRepositories = new ConcurrentDictionary<Type, object>();

        public Guid Id => this.id;

        protected abstract IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        protected IEnumerable<KeyValuePair<Type, object>> CachedRepositories => this.cacheRepositories;

        public abstract Task CommitAsync();
        public abstract void Commit();

        public IRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot
        {
            return (IRepository<TAggregateRoot>)this.cacheRepositories.GetOrAdd(typeof(TAggregateRoot), this.CreateRepository<TAggregateRoot>());
        }
    }
}
