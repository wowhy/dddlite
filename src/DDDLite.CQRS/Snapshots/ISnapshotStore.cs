namespace DDDLite.CQRS.Snapshots
{
  using System;
  using System.Threading.Tasks;

  public interface ISnapshotStore
  {
    Task<TSnapshot> GetAsync<TSnapshot>(Guid id)
      where TSnapshot : class, ISnapshot;

    Task SaveAsync<TSnapshot>(TSnapshot snapshot)
      where TSnapshot : class, ISnapshot;
  }
}