namespace DDDLite.CQRS.Events
{
  using System;
  using DDDLite.CQRS.Messages;

  public interface IEvent : IMessage
  {
    Guid Id { get; set; }

    Guid AggregateRootId { get; set; }

    long RowVersion { get; set; }

    DateTime Timestamp { get; set; }
  }
}