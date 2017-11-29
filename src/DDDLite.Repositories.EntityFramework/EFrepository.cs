namespace DDDLite.Repositories.EntityFramework
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using DDDLite.Domain;
  using DDDLite.Querying;
  using DDDLite.Specifications;
  using Microsoft.EntityFrameworkCore;

  public class EFRepository<TEntity, TKey> : Repository<TEntity, TKey>, IEFRepository<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    private readonly DbContext context;

    public EFRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
      context = unitOfWork as DbContext ?? throw new ArgumentNullException(nameof(context));
    }

    public DbContext Context => this.context;

    public override async Task InsertAsync(TEntity entity)
    {
      await Context.Set<TEntity>().AddAsync(entity);
    }

    public override Task UpdateAsync(TEntity entity)
    {
      Context.Update(entity);
      return Task.CompletedTask;
    }

    public override Task DeleteAsync(TEntity entity)
    {
      Context.Remove(entity);
      return Task.CompletedTask;
    }

    public override Task<TEntity> GetByIdAsync(TKey id, params string[] includes)
    {
      if (includes != null)
      {
        var query = Context.Set<TEntity>().AsQueryable();
        foreach (var include in includes)
        {
          query = query.Include(include);
        }

        return query.Where(k => k.Id.Equals(id)).FirstOrDefaultAsync();
      }

      return Context.Set<TEntity>().FindAsync(id);
    }

    public override IQueryable<TEntity> Search(Specification<TEntity> filter, SortSpecification<TEntity> sorter, params string[] includes)
    {
      if (filter == null)
      {
        filter = Specification<TEntity>.Any();
      }

      if (sorter == null)
      {
        sorter = SortSpecification<TEntity>.None;
      }

      var query = Context.Set<TEntity>().Where(filter);
      if (includes != null)
      {
        foreach (var include in includes)
        {
          query = query.Include(include);
        }
      }

      if (sorter.Count > 0)
      {
        var sorts = sorter.Specifications.ToList();
        var orderedQuery = sorts[0].Item2 == SortDirection.Asc
                                   ? query.OrderBy(sorts[0].Item1)
                                   : query.OrderByDescending(sorts[0].Item1);
        for (var i = 1; i < sorts.Count; i++)
        {
          orderedQuery = sorts[i].Item2 == SortDirection.Asc
                 ? orderedQuery.OrderBy(sorts[i].Item1)
                 : orderedQuery.OrderByDescending(sorts[i].Item1);
        }

        query = orderedQuery;
      }

      return query;
    }
  }
}
