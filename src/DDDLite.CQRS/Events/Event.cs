using System;

namespace DDDLite.CQRS.Events
{
  public class Event : IEvent
  {
    public Event() 
    {
      this.Id = SequentialGuid.Create();
    }

    public Guid Id { get; set; }

    public Guid AggregateRootId { get; set; }

    public long RowVersion { get; set; }

    public DateTime Timestamp { get; set; }
  }
}