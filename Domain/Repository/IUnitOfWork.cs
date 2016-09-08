namespace Domain.Repository
{
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        bool Committed { get; }

        void RegisterInserted<TEntity>(TEntity entity)
            where TEntity : class;

        void RegisterUpdated<TEntity>(TEntity entity)
            where TEntity : class;

        void RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class;

        void RegisterUnchanged<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> CommitAsync();

        bool Commit();

        void Rollback();
    }
}
