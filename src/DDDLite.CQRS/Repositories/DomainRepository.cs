namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.CQRS.Events;
  using DDDLite.Exception;

  public class DomainRepository<TEventSource> : IDomainRepository<TEventSource>
    where TEventSource : class, IEventSource, new()
  {
    private readonly IEventStore storage;
    private readonly IEventPublisher publisher;

    public DomainRepository(IEventStore storage, IEventPublisher publisher)
    {
      this.storage = storage;
      this.publisher = publisher;
    }

    public virtual async Task<TEventSource> GetByIdAsync(Guid id)
    {
      return await RestoreAggregateRootAsync(new TEventSource() { Id = id });
    }

    public virtual async Task SaveAsync(TEventSource aggregateRoot)
    {
      if ((await storage.GetAsync<TEventSource>(aggregateRoot.Id, aggregateRoot.RowVersion)).Any())
      {
        throw new ConcurrencyException(null);
      }

      var changes = aggregateRoot.FlushUncommitedChanges();
      await storage.SaveAsync<TEventSource>(changes);

      if (this.publisher != null)
      {
        foreach (var @event in changes)
        {
          await this.publisher.PublishAsync(@event);
        }
      }
    }

    protected async virtual Task<TEventSource> RestoreAggregateRootAsync(TEventSource aggregateRoot)
    {
      var events = await this.storage.GetAsync<TEventSource>(aggregateRoot.Id, aggregateRoot.RowVersion);
      if (!events.Any())
      {
        throw new AggregateRootNotFoundException<Guid>(aggregateRoot.Id);
      }

      aggregateRoot.LoadFromHistory(events);

      return aggregateRoot;
    }
  }
}