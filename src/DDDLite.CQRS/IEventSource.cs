namespace DDDLite.CQRS
{
  using System;
  using System.Collections.Generic;

  using DDDLite.Domain;
  using DDDLite.CQRS.Events;

  public interface IEventSource : IAggregateRoot<Guid>
  {
    IEvent[] GetUncommittedChanges();

    IEvent[] FlushUncommitedChanges();

    void LoadFromHistory(IEnumerable<IEvent> histories);
  }
}