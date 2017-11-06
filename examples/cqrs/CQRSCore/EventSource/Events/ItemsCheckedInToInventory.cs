namespace CQRSCore.EventSource.Events
{

  using System;
  using DDDLite.CQRS.Events;
  public class ItemsCheckedInToInventory : Event
  {
    private int count;

    public ItemsCheckedInToInventory(Guid id, int count)
    {
      this.Id = id;
      this.Count = count;
    }

    public int Count { get => count; set => count = value; }
  }
}