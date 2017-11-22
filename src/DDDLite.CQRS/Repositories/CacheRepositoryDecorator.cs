namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.Serialization;
  using Microsoft.Extensions.Caching.Distributed;
  using Newtonsoft.Json;

  public class CacheRepositoryDecorator<TEventSource> : IDomainRepository<TEventSource>
    where TEventSource : class, IEventSource, new()
  {
    private readonly IDomainRepository<TEventSource> repository;
    private readonly IDistributedCache cache;
    private readonly IObjectSerializer serializer;

    private readonly string keyPrefix = "__es__";

    public CacheRepositoryDecorator(
      IDistributedCache cache,
      IDomainRepository<TEventSource> repository,
      IObjectSerializer serializer)
    {
      this.repository = repository;
      this.cache = cache;
      this.serializer = serializer;
    }

    public async virtual Task<TEventSource> GetByIdAsync(Guid id)
    {
      var aggregateRoot = await this.GetFromCacheAsync(id);
      if (aggregateRoot != null)
      {
        return aggregateRoot;
      }

      aggregateRoot = await this.repository.GetByIdAsync(id);
      await this.SaveCacheAsync(aggregateRoot);
      return aggregateRoot;
    }

    public async virtual Task SaveAsync(TEventSource aggregateRoot)
    {
      await this.repository.SaveAsync(aggregateRoot);
      await this.SaveCacheAsync(aggregateRoot);
    }

    private async Task<TEventSource> GetFromCacheAsync(Guid id)
    {
      try
      {
        var value = await this.cache.GetAsync(keyPrefix + id);
        if (value == null)
        {
          return null;
        }

        return serializer.Deserialize<TEventSource>(value);
      }
      catch
      {
      }

      return null;
    }

    private Task SaveCacheAsync(TEventSource aggregateRoot)
    {
      try
      {
        return this.cache.SetAsync(keyPrefix + aggregateRoot, serializer.Serialize(aggregateRoot));
      }
      catch
      {
      }

      return Task.CompletedTask;
    }
  }
}