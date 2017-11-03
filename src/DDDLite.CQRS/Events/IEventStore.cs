namespace DDDLite.CQRS.Events
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface IEventStore
  {
    Task SaveAsync(IEnumerable<IEvent> events);

    Task<IEnumerable<IEvent>> GetAsync(Guid aggregateRootId, long fromVersion);
  }
}