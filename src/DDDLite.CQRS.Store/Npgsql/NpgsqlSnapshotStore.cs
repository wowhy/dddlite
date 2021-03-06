namespace DDDLite.CQRS.Store.Npgsql
{
  using System;
  using System.Threading.Tasks;
  using System.Linq;
  using DDDLite.CQRS.Snapshots;
  using Marten;

  public class NpgsqlSnapshotStore : ISnapshotStore
  {
    private readonly DocumentStore store;

    public NpgsqlSnapshotStore(string connectionString)
    {
      this.store = DocumentStore.For(config =>
      {
        config.Connection(connectionString);
        config.DatabaseSchemaName = "snapshots";
      });
    }

    public async Task<TSnapshot> GetByIdAsync<TSnapshot>(Guid id)
     where TSnapshot : class, ISnapshot
    {
      using (var session = store.LightweightSession())
      {
        return await session.Query<TSnapshot>().Where(k => k.Id == id).FirstOrDefaultAsync();
      }
    }

    public async Task SaveAsync<TSnapshot>(TSnapshot snapshot)
     where TSnapshot : class, ISnapshot
    {
      using (var session = store.LightweightSession())
      {
        session.Store(snapshot);
        await session.SaveChangesAsync();
      }
    }
  }
}