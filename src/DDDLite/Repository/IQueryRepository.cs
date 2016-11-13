namespace DDDLite.Repository
{
    using System;
    using System.Linq;

    using Specifications;

    public interface IQueryRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IQueryable<TAggregateRoot> QueryModel { get; }

        TDTO GetById<TDTO>(Guid id) where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>() where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification) where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification) where TDTO : class, new();
    }
}
