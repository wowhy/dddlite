namespace DDDLite.Querying
{
    using System;
    using System.Linq;

    using Domain;
    using Repository;
    using Specifications;

    public interface IQueryService
    {
        IQueryRepositoryContext Context { get; }
    }

    public interface IQueryService<TAggregateRoot> : IQueryService
        where TAggregateRoot : class, IAggregateRoot
    {
        IQueryRepository<TAggregateRoot> Repository { get; }

        TAggregateRoot GetById(Guid id);

        TDTO GetById<TDTO>(Guid id);

        IQueryable<TAggregateRoot> FindAll();

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> FindAll(
            Specification<TAggregateRoot> specification,
            SortSpecification<TAggregateRoot> sortSpecification
        );

        PagedResult<TAggregateRoot> Page(int page, int limit);

        PagedResult<TAggregateRoot> Page(
            int page,
            int limit,
            Specification<TAggregateRoot> specification
        );

        PagedResult<TAggregateRoot> Page(
            int page,
            int limit,
            Specification<TAggregateRoot> specification,
            SortSpecification<TAggregateRoot> sortSpecification
        );
    }
}
