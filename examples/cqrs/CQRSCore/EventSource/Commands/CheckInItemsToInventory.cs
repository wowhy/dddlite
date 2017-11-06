namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;
  public class CheckInItemsToInventory : Command
  {
    public readonly int Count;

    public CheckInItemsToInventory(Guid id, int count, int originalVersion)
    {
      this.Id = id;
      Count = count;
      this.OriginalVersion = originalVersion;
    }
  }
}