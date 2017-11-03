namespace DDDLite.CQRS.Events
{
  using System;

  public class EventDescriptor<TEvent>
    where TEvent : class, IEvent
  {
    public EventDescriptor(TEvent data)
    {
      this.Data = data;
      this.Id = data.Id;
      this.AggregateRootId = data.AggregateRootId;
      this.Timestamp = data.Timestamp;
      this.RowVersion = data.RowVersion;
      this.OperatorId = data.OperatorId;
    }

    public Guid Id { get; set; }

    public Guid AggregateRootId { get; set; }

    public DateTime Timestamp { get; set; }

    public long RowVersion { get; set; }

    public string OperatorId { get; set; }

    public TEvent Data { get; set; }
  }
}