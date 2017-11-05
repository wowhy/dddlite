namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;
  public class RenameInventoryItem : Command
  {
    public readonly string NewName;

    public RenameInventoryItem(Guid id, string newName, int originalVersion)
    {
      this.AggregateRootId = id;
      NewName = newName;
      this.RowVersion = originalVersion;
    }
  }
}