namespace Domain.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core;
    using Specifications;

    public interface IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TEntity Get(TKey key);

        Task<TEntity> GetAsync(TKey key);

        IQueryable<TEntity> FindAll();

        IQueryable<TEntity> FindAll(Specification<TEntity> specification);

        IQueryable<TEntity> FindAll(Specification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        bool Exists(Specification<TEntity> specification);
    }
}
