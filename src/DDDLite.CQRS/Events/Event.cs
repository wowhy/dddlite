using System;

namespace DDDLite.CQRS.Events
{
  public class Event : IEvent
  {
    public Guid Id { get; set; }

    public long Version { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public long OriginalVersion => this.Version - 1;

    public DateTime Timestamp { get; set; }

    public string OperatorId { get; set; }
  }
}