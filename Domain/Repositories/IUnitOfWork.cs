namespace Domain.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }

        Task CommitAsync();

        void Commit();
    }
}
