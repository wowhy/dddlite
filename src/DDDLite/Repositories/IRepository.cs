﻿namespace DDDLite.Repositories
{
    using DDDLite.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using DDDLite.Specifications;
    using DDDLite.Querying;

    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TAggregateRoot> GetByIdAsync(Guid id, params string[] includes);

        Task AddAsync(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);

        Task DeleteAsync(TAggregateRoot entity);

        bool Exists(Specification<TAggregateRoot> filter);

        IQueryable<TAggregateRoot> Search(params string[] includes);

        IQueryable<TAggregateRoot> Search(
            Specification<TAggregateRoot> filter,
            SortSpecification<TAggregateRoot> sorter,
            params string[] includes);

        IQueryable<TAggregateRoot> Search(
            Specification<TAggregateRoot> filter,
            params string[] includes);

        IQueryable<TAggregateRoot> Search(
            SortSpecification<TAggregateRoot> sorter,
            params string[] includes);

        PagedResult<TAggregateRoot> PagedSearch(int top, int skip, Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter, params string[] includes);

        PagedResult<TAggregateRoot> PagedSearch(int top, int skip, SortSpecification<TAggregateRoot> sorter, params string[] includes);
    }
}