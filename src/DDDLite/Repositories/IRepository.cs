namespace DDDLite.Repositories
{
    using DDDLite.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using DDDLite.Specifications;
    using DDDLite.Querying;

    public interface IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TAggregateRoot> GetByIdAsync(TKey id, params string[] includes);

        Task InsertAsync(TAggregateRoot entity);

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