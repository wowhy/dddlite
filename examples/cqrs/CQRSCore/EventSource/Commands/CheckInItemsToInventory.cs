namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;
  public class CheckInItemsToInventory : Command
  {
    public readonly int Count;

    public CheckInItemsToInventory(Guid id, int count, int originalVersion)
    {
      this.AggregateRootId = id;
      Count = count;
      this.RowVersion = originalVersion;
    }
  }
}