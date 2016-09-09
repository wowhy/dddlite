namespace Domain.Repositories
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

        protected abstract IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        protected IEnumerable<KeyValuePair<Type, object>> CachedRepositories => this.cacheRepositories;

        public abstract Task CommitAsync();
        public abstract void Commit();

        public IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return (IRepository<TKey, TEntity>)this.cacheRepositories.GetOrAdd(typeof(TEntity), this.CreateRepository<TKey, TEntity>());
        }
    }
}
