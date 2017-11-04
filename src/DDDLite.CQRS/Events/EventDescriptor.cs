namespace DDDLite.CQRS.Events
{
  using System;

  public class EventDescriptor<TEventSource>
    where TEventSource : class, IEventSource
  {
    public EventDescriptor(IEvent data)
    {
      this.Data = data;

      this.Id = data.Id;
      this.AggregateRootId = data.AggregateRootId;
      this.Timestamp = data.Timestamp;
      this.RowVersion = data.RowVersion;
      this.OperatorId = data.OperatorId;

      this.EventType = data.GetType().FullName;
    }

    public Guid Id { get; set; }

    public Guid AggregateRootId { get; set; }

    public DateTime Timestamp { get; set; }

    public long RowVersion { get; set; }

    public string OperatorId { get; set; }

    public string EventType { get; set; }

    public object Data { get; set; }
  }
}