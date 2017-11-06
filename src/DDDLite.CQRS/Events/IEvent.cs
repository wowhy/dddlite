namespace DDDLite.CQRS.Events
{
  using System;
  using DDDLite.CQRS.Messaging;

  public interface IEvent : IMessage
  {
    Guid Id { get; set; }

    long Version { get; set; }

    long OriginalVersion { get; }

    DateTime Timestamp { get; set; }

    string OperatorId { get; set; }
  }
}