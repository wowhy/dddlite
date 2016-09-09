namespace Domain.Repositories
{
    using System;
    using Core;

    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
    }
}
