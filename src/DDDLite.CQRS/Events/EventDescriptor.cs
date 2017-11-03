namespace DDDLite.CQRS.Events
{
  using System;

  public class EventDescriptor<TEventSource>
    where TEventSource : class, IEventSource
  {
    public EventDescriptor(object data)
    {
      this.Data = data;

      var e = (IEvent)data;
      this.Id = e.Id;
      this.AggregateRootId = e.AggregateRootId;
      this.Timestamp = e.Timestamp;
      this.RowVersion = e.RowVersion;
      this.OperatorId = e.OperatorId;
    }

    public Guid Id { get; set; }

    public Guid AggregateRootId { get; set; }

    public DateTime Timestamp { get; set; }

    public long RowVersion { get; set; }

    public string OperatorId { get; set; }

    public object Data { get; set; }
  }
}