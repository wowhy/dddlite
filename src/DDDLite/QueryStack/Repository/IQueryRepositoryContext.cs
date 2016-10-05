namespace DDDLite.QueryStack.Repository
{
    using System;
    using System.Linq;

    using Domain;

    public interface IQueryRepositoryContext
    {
        Guid Id { get; }

        IQueryable<TAggregateRoot> GetQueryableModel<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        IQueryRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
