namespace DDDLite.Domain
{
    using System;
    using System.Linq;

    public interface IQueryRepositoryContext
    {
        Guid Id { get; }

        IQueryable<TAggregateRoot> GetQueryableModel<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        IQueryRepository<TAggregateRoot> GetRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;
    }
}
