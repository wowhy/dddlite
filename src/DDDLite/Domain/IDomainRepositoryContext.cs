namespace DDDLite.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface IDomainRepositoryContext
    {
        Guid Id { get; }

        void Commit();

        Task CommitAsync();

        IDomainRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
