namespace DDDLite.CQRS.Events
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface IEventStore
  {
    Task SaveAsync<TEventSource>(IEnumerable<IEvent> events)
      where TEventSource : class, IEventSource;

    Task<IEnumerable<IEvent>> GetByIdAsync<TEventSource>(Guid aggregateRootId, long fromVersion)
      where TEventSource : class, IEventSource;
  }
}