namespace DDDLite.Repositories
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
        Task<TAggregateRoot> GetByIdAsync(Guid id);

        Task AddAsync(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);

        Task DeleteAsync(TAggregateRoot entity);

        IQueryable<TAggregateRoot> Search();

        IQueryable<TAggregateRoot> Search(
            Specification<TAggregateRoot> filter,
            SortSpecification<TAggregateRoot> sorter);

        IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter);

        IQueryable<TAggregateRoot> Search(SortSpecification<TAggregateRoot> sorter);

        PagedResult<TAggregateRoot> PagedSearch(int page, int limit, Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter);

        PagedResult<TAggregateRoot> PagedSearch(int page, int limit, SortSpecification<TAggregateRoot> sorter);
    }
}