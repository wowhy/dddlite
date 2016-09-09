namespace Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Specifications;

    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRepositoryContext context;

        protected Repository(IRepositoryContext context)
        {
            this.context = context;
        }

        public IRepositoryContext Context => this.context;

        public abstract TEntity Get(TKey key);

        public abstract Task<TEntity> GetAsync(TKey key);

        public abstract IQueryable<TEntity> FindAll();

        public abstract IQueryable<TEntity> FindAll(Specification<TEntity> specification);

        public abstract IQueryable<TEntity> FindAll(Specification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification);

        public abstract void Add(TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract void Remove(TEntity entity);

        public abstract bool Exists(Specification<TEntity> specification);
    }
}
