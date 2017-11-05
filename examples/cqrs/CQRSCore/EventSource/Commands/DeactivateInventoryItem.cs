namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;
  public class DeactivateInventoryItem : Command
  {
    public DeactivateInventoryItem(Guid id, int originalVersion)
    {
      this.AggregateRootId = id;
      this.RowVersion = originalVersion;
    }
  }
}