namespace DDDLite.CQRS.Npgsql
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Snapshots;

  public class NpgsqlSnapshotStore : ISnapshotStore
  {
    public NpgsqlSnapshotStore()
    {
    }

    Task<TSnapshot> ISnapshotStore.GetAsync<TSnapshot>(Guid id)
    {
      throw new NotImplementedException();
    }

    Task ISnapshotStore.SaveAsync<TSnapshot>(TSnapshot snapshot)
    {
      throw new NotImplementedException();
    }
  }
}