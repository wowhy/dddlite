namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.Exception;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Snapshots;

  public class SnapshotRepository<TEventSource, TSnapshot> : DomainRepository<TEventSource>
   where TEventSource : class, IEventSource, ISnapshotDecorator<TSnapshot>, new()
   where TSnapshot : class, ISnapshot
  {
    private readonly ISnapshotStore snapshotStore;

    protected ISnapshotStore SnapshotStore => this.snapshotStore;

    public SnapshotRepository(
      IEventStore eventStore,
      IEventPublisher publisher,
      ISnapshotStore snapshotStore) :
      base(eventStore, publisher)
    {
      this.snapshotStore = snapshotStore;
    }

    public async override Task<TEventSource> GetByIdAsync(Guid id)
    {
      return await RestoreAggregateRootFromSnapshotAsync(new TEventSource { Id = id });
    }

    public async override Task SaveAsync(TEventSource aggregateRoot)
    {
      await base.SaveAsync(aggregateRoot);
      await SaveSnapshotAsync(aggregateRoot);
    }

    protected async virtual Task<TEventSource> CreateAggregateRootAsync(Guid id)
    {
      var aggregateRoot = new TEventSource
      {
        Id = id
      };

      var snapshot = await this.snapshotStore.GetAsync<TSnapshot>(id);
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
      var snapshot = await this.snapshotStore.GetAsync<TSnapshot>(aggregateRoot.Id);
      if (snapshot != null)
      {
        aggregateRoot.RestoreFromSnapshot(snapshot);
        var events = (await this.Storage.GetAsync<TEventSource>(aggregateRoot.Id, aggregateRoot.Version)).ToList();
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