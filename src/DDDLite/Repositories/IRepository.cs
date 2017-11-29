namespace DDDLite.Repositories
{
  using DDDLite.Domain;
  using System;
  using System.Threading.Tasks;
  using System.Linq;
  using DDDLite.Specifications;
  using DDDLite.Querying;

  public interface IRepository<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    IUnitOfWork UnitOfWork { get; }

    Task<TEntity> GetByIdAsync(TKey id, params string[] includes);

    Task InsertAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    bool Exists(Specification<TEntity> filter);

    IQueryable<TEntity> Search(params string[] includes);

    IQueryable<TEntity> Search(
        Specification<TEntity> filter,
        SortSpecification<TEntity> sorter,
        params string[] includes);

    IQueryable<TEntity> Search(
        Specification<TEntity> filter,
        params string[] includes);

    IQueryable<TEntity> Search(
        SortSpecification<TEntity> sorter,
        params string[] includes);

    PagedResult<TEntity> PagedSearch(int top, int skip, Specification<TEntity> filter, SortSpecification<TEntity> sorter, params string[] includes);

    PagedResult<TEntity> PagedSearch(int top, int skip, SortSpecification<TEntity> sorter, params string[] includes);
  }
}