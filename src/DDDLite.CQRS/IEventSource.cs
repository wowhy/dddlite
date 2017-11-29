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

    IEvent[] GetUncommittedChanges();

    IEvent[] FlushUncommitedChanges();

    void LoadFromHistory(IEnumerable<IEvent> histories);

    void ApplyChange(IEvent @event);
  }
}