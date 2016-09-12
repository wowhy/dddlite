namespace Domain.Repositories
{
    using System;
    using Core;

    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        IRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
