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

    protected IEventStore Storage => this.storage;
    protected IEventPublisher Publisher => this.publisher;

    public DomainRepository(IEventStore storage, IEventPublisher publisher)
    {
      this.storage = storage;
      this.publisher = publisher;
    }

    public virtual async Task<TEventSource> GetByIdAsync(Guid id)
    {
      var aggregateRoot = await RestoreAggregateRootAsync(new TEventSource() { Id = id });
      return aggregateRoot;
    }

    public virtual async Task<TEventSource> GetByIdAsync(Guid id, long expectedVersion)
    {
      var aggregateRoot = await this.GetByIdAsync(id);
      if (aggregateRoot.Version != expectedVersion)
      {
        throw new ConcurrencyException();
      }
      return aggregateRoot;
    }

    public virtual async Task SaveAsync(TEventSource aggregateRoot, long expectedVersion)
    {
      if ((await storage.GetAsync<TEventSource>(aggregateRoot.Id, expectedVersion)).Any())
      {
        throw new ConcurrencyException();
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
      var events = (await this.storage.GetAsync<TEventSource>(aggregateRoot.Id, aggregateRoot.Version)).ToList();
      if (events.Count == 0)
      {
        throw new AggregateRootNotFoundException<Guid>(aggregateRoot.Id);
      }
      aggregateRoot.LoadFromHistory(events);
      return aggregateRoot;
    }
  }
}