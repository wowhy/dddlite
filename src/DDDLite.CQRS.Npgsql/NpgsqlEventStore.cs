namespace DDDLite.CQRS.Npgsql
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System.Linq;
  using DDDLite.CQRS.Events;
  using Marten;
  using Newtonsoft.Json;
  using System.IO;
  using Newtonsoft.Json.Linq;

  public class NpgsqlEventStore : IEventStore
  {
    private readonly DocumentStore store;

    public NpgsqlEventStore(string connectionString)
    {
      this.store = DocumentStore.For(config =>
      {
        config.Connection(connectionString);
      });
    }

    public async Task<IEnumerable<IEvent>> GetAsync<TEventSource>(Guid aggregateRootId, long fromVersion)
      where TEventSource : class, IEventSource
    {
      using (var session = store.LightweightSession())
      {
        var documents = session.Query<EventDescriptor<TEventSource>>().Where(k => k.AggregateRootId == aggregateRootId && k.RowVersion > fromVersion);
        var jsons = await documents.Select(k => k.Data).ToListAsync();
        return jsons.Select(k =>
        {
          var obj = (JObject)JsonConvert.DeserializeObject(k.ToString());
          var type = Type.GetType(obj["$type"].ToString());
          return (IEvent)obj.ToObject(type);
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
        // session.DocumentStore
        await session.SaveChangesAsync();
      }
    }
  }
}