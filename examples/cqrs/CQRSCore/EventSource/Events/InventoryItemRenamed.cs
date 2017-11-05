namespace CQRSCore.EventSource.Events
{

  using System;
  using DDDLite.CQRS.Events;
  public class InventoryItemRenamed : Event
  {
    private string newName;

    public InventoryItemRenamed(Guid id, string newName)
    {
      this.AggregateRootId = id;
      this.NewName = newName;
    }

    public string NewName { get => newName; set => newName = value; }
  }
}