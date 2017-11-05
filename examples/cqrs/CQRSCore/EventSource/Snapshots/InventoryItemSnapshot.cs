namespace CQRSCore.EventSource.Snapshots
{
  using DDDLite.CQRS.Snapshots;
  public class InventoryItemSnapshot : Snapshot
  {
    public bool Activated { get; set; }
  }
}