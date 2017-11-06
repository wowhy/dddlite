using System;

namespace DDDLite.CQRS.Events
{
  public class Event : IEvent
  {
    public Guid Id { get; set; }

    public long RowVersion { get; set; }

    public DateTime Timestamp { get; set; }

    public string OperatorId { get; set; }
  }
}