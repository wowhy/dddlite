namespace DDDLite.CQRS
{
  using System;
  using System.Collections.Generic;

  using DDDLite.Domain;
  using DDDLite.CQRS.Events;

  public interface IEventSource : ILogicalDelete
  {
    Guid Id { get; set; }

    long Version { get; set; }

    IEvent[] GetUndispatchedEvents();

    IEvent[] FlushUndispatchedEvents();

    void LoadFromHistory(IEnumerable<IEvent> histories);

    void ApplyEvent(IEvent @event);
  }
}