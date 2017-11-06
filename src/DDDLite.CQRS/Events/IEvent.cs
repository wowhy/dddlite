namespace DDDLite.CQRS.Events
{
  using System;
  using DDDLite.CQRS.Messages;

  public interface IEvent : IMessage
  {
    Guid Id { get; set; }

    long RowVersion { get; set; }

    DateTime Timestamp { get; set; }

    string OperatorId { get; set; }
  }
}