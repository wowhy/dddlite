
namespace CQRSCore.EventSource.Events
{

  using System;
  using DDDLite.CQRS.Events;
  public class InventoryItemDeactivated : Event
  {

    public InventoryItemDeactivated(Guid id)
    {
      this.AggregateRootId = id;
    }
  }
}