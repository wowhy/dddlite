namespace DDDLite.CommandStack.Repository
{
    using System;
    using System.Threading.Tasks;

    using Domain;

    public interface IDomainRepositoryContext
    {
        Guid Id { get; }

        void Commit();

        Task CommitAsync();

        IDomainRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
