namespace DDDLite.Repository
{
    using System;
    using System.Linq;

    using Domain;
    using Specifications;

    public interface IQueryRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IQueryRepositoryContext Context { get; }

        TDTO GetById<TDTO>(Guid id) where TDTO : class, new();

        IQueryable<TDTO> FindAll<TDTO>() where TDTO : class, new();

        IQueryable<TDTO> FindAll<TDTO>(Specification<TAggregateRoot> specification) where TDTO : class, new();

        IQueryable<TDTO> FindAll<TDTO>(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification) where TDTO : class, new();
    }
}
