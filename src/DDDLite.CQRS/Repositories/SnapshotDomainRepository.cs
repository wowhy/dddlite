namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.Exception;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Snapshots;

  public class SnapshotDomainRepository<TEventSource, TSnapshot> : DomainRepository<TEventSource>
   where TEventSource : class, IEventSource, ISnapshotDecorator<TSnapshot>, new()
   where TSnapshot : class, ISnapshot
  {
    private readonly ISnapshotStore snapshotStore;

    protected ISnapshotStore SnapshotStore => this.snapshotStore;

    public SnapshotDomainRepository(
      IEventStore eventStore,
      IEventPublisher publisher,
      ISnapshotStore snapshotStore) :
      base(eventStore, publisher)
    {
      this.snapshotStore = snapshotStore;
    }

    public override Task<TEventSource> GetByIdAsync(Guid id)
    {
      return RestoreAggregateRootFromSnapshotAsync(new TEventSource { Id = id });
    }

    public async override Task<TEventSource> GetByIdAsync(Guid id, long expectedVersion)
    {
      var aggregateRoot = await this.GetByIdAsync(id);
      if (aggregateRoot.Version != expectedVersion)
      {
        throw new ConcurrencyException();
      }
      return aggregateRoot;
    }

    public async override Task SaveAsync(TEventSource aggregateRoot, long expectedVersion)
    {
      await base.SaveAsync(aggregateRoot, expectedVersion);
      await SaveSnapshotAsync(aggregateRoot);
    }

    protected async virtual Task<TEventSource> CreateAggregateRootAsync(Guid id)
    {
      var aggregateRoot = new TEventSource
      {
        Id = id
      };

      var snapshot = await this.snapshotStore.GetByIdAsync<TSnapshot>(id);
      if (snapshot != null)
      {
        aggregateRoot.RestoreFromSnapshot(snapshot);
      }

      return aggregateRoot;
    }

    protected async virtual Task SaveSnapshotAsync(TEventSource aggregateRoot)
    {
      var snapshot = aggregateRoot.GetSnapshot();
      await this.snapshotStore.SaveAsync(snapshot);
    }

    protected async virtual Task<TEventSource> RestoreAggregateRootFromSnapshotAsync(TEventSource aggregateRoot)
    {
      var snapshot = await this.snapshotStore.GetByIdAsync<TSnapshot>(aggregateRoot.Id);
      if (snapshot != null)
      {
        aggregateRoot.RestoreFromSnapshot(snapshot);
        var events = (await this.Storage.GetByIdAsync<TEventSource>(aggregateRoot.Id, aggregateRoot.Version)).ToList();
        if (events.Count > 0)
        {
          aggregateRoot.LoadFromHistory(events);
        }
        return aggregateRoot;
      }
      else
      {
        return await base.RestoreAggregateRootAsync(aggregateRoot);
      }
    }
  }
}