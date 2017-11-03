namespace DDDLite.CQRS.Snapshots
{
  public interface ISnapshotDecorator<TSnapshot>
    where TSnapshot : class, ISnapshot
  {
    TSnapshot GetSnapshot();

    void RestoreFromSnapshot(TSnapshot snapshot);
  }
}