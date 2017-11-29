namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  public interface ISagaStore
  {
    Task<TSaga> FindAsync<TSaga>(Expression<Func<TSaga, bool>> predicate, bool includeCompleted = false) where TSaga : class, ISaga, new();

    Task SaveAsync<TSaga>(TSaga saga) where TSaga : class, ISaga, new();
  }
}