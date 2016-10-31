namespace DDDLite.Repository
{
    using System;
    using System.Linq;
    using Domain;

    public interface IQueryRepositoryContext
    {
        Guid Id { get; }

        IQueryRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        IQueryable<TAggregateRoot> GetQueryModel<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}
