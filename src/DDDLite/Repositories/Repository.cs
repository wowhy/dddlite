namespace DDDLite.Repositories
{
  using DDDLite.Domain;
  using System;
  using System.Threading.Tasks;
  using DDDLite.Specifications;
  using System.Linq;
  using DDDLite.Querying;
  using System.Linq.Expressions;

  public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    protected Repository(IUnitOfWork unitOfWork)
    {
      UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IUnitOfWork UnitOfWork { get; protected set; }

    public abstract Task InsertAsync(TEntity entity);
    public abstract Task UpdateAsync(TEntity entity);
    public abstract Task DeleteAsync(TEntity entity);
    public abstract Task<TEntity> GetByIdAsync(TKey id, params string[] includes);
    public abstract IQueryable<TEntity> Search(Specification<TEntity> filter, SortSpecification<TEntity> sorter, params string[] includes);

    public virtual IQueryable<TEntity> Search(params string[] includes)
    {
      return this.Search(Specification<TEntity>.Any(), SortSpecification<TEntity>.None, includes);
    }

    public virtual IQueryable<TEntity> Search(Specification<TEntity> filter, params string[] includes)
    {
      return this.Search(filter, SortSpecification<TEntity>.None, includes);
    }

    public virtual IQueryable<TEntity> Search(SortSpecification<TEntity> sorter, params string[] includes)
    {
      return this.Search(Specification<TEntity>.Any(), sorter, includes);
    }

    public virtual IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> predicate, Sorter sorter, params string[] includes)
    {
      return this.Search(
        Specification<TEntity>.Eval(predicate),
        sorter == null ? SortSpecification<TEntity>.None : new SortSpecification<TEntity>(sorter.Property, sorter.SortOrder),
        includes);
    }

    public virtual IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
      return this.Search(
        Specification<TEntity>.Eval(predicate),
        includes);
    }

    public virtual bool Exists(Specification<TEntity> filter)
    {
      return this.Search(filter).Any();
    }

    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
      return this.Exists(Specification<TEntity>.Eval(predicate));
    }
  }
}
