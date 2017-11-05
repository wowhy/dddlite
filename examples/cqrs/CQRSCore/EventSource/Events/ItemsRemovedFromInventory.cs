namespace CQRSCore.EventSource.Events
{

  using System;
  using DDDLite.CQRS.Events;
  public class ItemsRemovedFromInventory : Event
  {
    private int count;

    public ItemsRemovedFromInventory(Guid id, int count)
    {
      this.Count = count;
    }

    public int Count { get => count; set => count = value; }
  }
}