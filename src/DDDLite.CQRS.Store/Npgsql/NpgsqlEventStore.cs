namespace DDDLite.CQRS.Store.Npgsql
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System.Linq;
  using DDDLite.CQRS.Events;
  using Marten;

  public class NpgsqlEventStore : IEventStore
  {
    private readonly DocumentStore store;

    public NpgsqlEventStore(string connectionString)
    {
      this.store = DocumentStore.For(config =>
      {
        config.Connection(connectionString);
        config.DatabaseSchemaName = "events";
      });
    }

    public async Task<IEnumerable<IEvent>> GetAsync<TEventSource>(Guid aggregateRootId, long fromVersion)
      where TEventSource : class, IEventSource
    {
      using (var session = store.LightweightSession())
      {
        var query = session.Query<EventDescriptor<TEventSource>>().Where(k => k.AggregateRootId == aggregateRootId && k.Version > fromVersion);
        var documents = await query.ToListAsync();
        return documents.Select(d =>
        {
          return (IEvent)d.Data;
        });
      }
    }

    public async Task SaveAsync<TEventSource>(IEnumerable<IEvent> events)
      where TEventSource : class, IEventSource
    {
      using (var session = store.LightweightSession())
      {
        var documents = events.Select(k => new EventDescriptor<TEventSource>(k));
        session.InsertObjects(documents);
        await session.SaveChangesAsync();
      }
    }
  }
}