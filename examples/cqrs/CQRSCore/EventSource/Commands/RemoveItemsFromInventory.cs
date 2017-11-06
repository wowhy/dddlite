namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;
  public class RemoveItemsFromInventory : Command
  {
    public readonly int Count;

    public RemoveItemsFromInventory(Guid id, int count, int originalVersion)
    {
      this.Id = id;
      Count = count;
      this.OriginalVersion = originalVersion;
    }
  }
}