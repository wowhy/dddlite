namespace DDDLite.Domain
{
    using System;
    using System.Linq;

    using Specifications;

    public interface IQueryRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IQueryRepositoryContext Context { get; }

        TAggregateRoot GetById(Guid id);

        bool Exist(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> FindAll();

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);
    }
}
