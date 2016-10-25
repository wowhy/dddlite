namespace DDDLite.Repository
{
    using System;

    using Domain;

    public interface IQueryRepositoryContext
    {
        Guid Id { get; }

        IQueryRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
