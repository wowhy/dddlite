namespace DDDLite.CQRS.Store.Npgsql
{
  using System;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Sagas;
  using DDDLite.CQRS.Snapshots;
  using Marten;

  public class NpgsqlSagaStore : ISagaStore
  {
    private readonly DocumentStore store;

    public NpgsqlSagaStore(string connectionString)
    {
      this.store = DocumentStore.For(config =>
      {
        config.Connection(connectionString);
        config.DatabaseSchemaName = "snapshots";
      });
    }

    public async Task<TSaga> FindAsync<TSaga>(Expression<Func<TSaga, bool>> predicate, bool includeCompleted)
        where TSaga : class, ISaga, new()
    {
      using (var session = store.LightweightSession())
      {
        var saga = await session.Query<TSaga>().Where(predicate).FirstOrDefaultAsync();
        return saga;
      }
    }

    public async Task SaveAsync<TSaga>(TSaga saga)
        where TSaga : class, ISaga, new()
    {
      using (var session = store.LightweightSession())
      {
        session.Store(saga);
        await session.SaveChangesAsync();
      }
    }
  }
}