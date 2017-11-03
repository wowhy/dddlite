using DDDLite.CQRS.Snapshots;

namespace DDDLite.CQRS
{
  public abstract class SnapshotEventSource<TSnapshot> : EventSource, ISnapshotDecorator<TSnapshot>
    where TSnapshot : class, ISnapshot
  {
    public abstract TSnapshot GetSnapshot();

    public abstract void RestoreFromSnapshot(TSnapshot snapshot);
  }
}