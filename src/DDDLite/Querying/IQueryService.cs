namespace DDDLite.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Domain;
    using Repository;

    public interface IQueryService
    {
        IQueryRepositoryContext Context { get; }
    }

    public interface IQueryService<TAggregateRoot> : IQueryService
        where TAggregateRoot : class, IAggregateRoot
    {
        IQueryRepository<TAggregateRoot> Repository { get; }

        TDTO GetById<TDTO>(Guid id) where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>() where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>(ICollection<Filter> filters) where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>(ICollection<Sorter> sorters) where TDTO : class, new();

        IQueryable<TDTO> Find<TDTO>(ICollection<Filter> filters, ICollection<Sorter> sorters) where TDTO : class, new();

        PagedResult<TDTO> Page<TDTO>(int page, int limit) where TDTO : class, new();

        PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Filter> filters) where TDTO : class, new();

        PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Sorter> sorters) where TDTO : class, new();

        PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Filter> filters, ICollection<Sorter> sorters) where TDTO : class, new();
    }
}
