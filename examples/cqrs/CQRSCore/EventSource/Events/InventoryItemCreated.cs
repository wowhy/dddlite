namespace CQRSCore.EventSource.Events
{
  using System;
  using DDDLite.CQRS.Events;

  public class InventoryItemCreated : Event
  {
    public readonly string Name;
    public InventoryItemCreated(Guid id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}