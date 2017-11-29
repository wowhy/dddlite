namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Linq.Expressions;
  using System.Threading.Tasks;
  using DDDLite.Specifications;

  public interface ISagaRepository<TSaga>
    where TSaga : class, ISaga, new()
  {
    Task<TSaga> GetByIdAsync(Guid sagaId);

    Task<TSaga> FindAsync(Expression<Func<TSaga, bool>> predicate, bool includeCompleted = false);

    Task<TSaga> FindAsync(Specification<TSaga> spec, bool includeCompleted = false);

    Task SaveAsync(TSaga saga);
  }
}