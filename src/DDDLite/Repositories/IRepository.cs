namespace DDDLite.Repositories
{
  using System;
  using System.Threading.Tasks;
  using System.Linq;
  using System.Linq.Expressions;
  using DDDLite.Domain;
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

    bool Exists(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> Search(params string[] includes);

    IQueryable<TEntity> Search(
        Specification<TEntity> filter,
        SortSpecification<TEntity> sorter,
        params string[] includes
    );

    IQueryable<TEntity> Search(
        Specification<TEntity> filter,
        params string[] includes
    );

    IQueryable<TEntity> Search(
        SortSpecification<TEntity> sorter,
        params string[] includes
    );

    IQueryable<TEntity> Search(
        Expression<Func<TEntity, bool>> predicate,
        Sorter sorter,
        params string[] includes
    );

    IQueryable<TEntity> Search(
        Expression<Func<TEntity, bool>> predicate,
        params string[] includes
    );
  }
}