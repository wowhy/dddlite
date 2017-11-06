namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Caching.Distributed;
  using Newtonsoft.Json;

  public class CacheRepositoryDecorator<TEventSource> : IDomainRepository<TEventSource>
    where TEventSource : class, IEventSource, new()
  {
    private readonly IDomainRepository<TEventSource> repository;
    private readonly IDistributedCache cache;
    private readonly JsonSerializerSettings settings;

    private readonly string keyPrefix = "__es__";

    public CacheRepositoryDecorator(IDistributedCache cache, IDomainRepository<TEventSource> repository)
    {
      this.repository = repository;
      this.cache = cache;
      this.settings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.Auto
      };
    }

    public async virtual Task<TEventSource> GetByIdAsync(Guid id)
    {
      var aggregateRoot = await this.TryGetCacheAsync(id);
      if (aggregateRoot != null)
      {
        return aggregateRoot;
      }

      aggregateRoot = await this.repository.GetByIdAsync(id);
      await this.TrySaveCacheAsync(aggregateRoot);
      return aggregateRoot;
    }

    public async virtual Task SaveAsync(TEventSource aggregateRoot)
    {
      await this.repository.SaveAsync(aggregateRoot);
      await this.TrySaveCacheAsync(aggregateRoot);
    }

    private async Task<TEventSource> TryGetCacheAsync(Guid id)
    {
      var value = await this.cache.GetStringAsync(keyPrefix + id);
      if (string.IsNullOrEmpty(value))
      {
        return null;
      }

      return JsonConvert.DeserializeObject<TEventSource>(value, settings);
    }

    private Task TrySaveCacheAsync(TEventSource aggregateRoot)
    {
      return this.cache.SetStringAsync(keyPrefix + aggregateRoot, JsonConvert.SerializeObject(aggregateRoot, settings));
    }
  }
}