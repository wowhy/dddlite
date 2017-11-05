namespace DDDLite.CQRS.Messaging.Redis
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Messaging.InMemory;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using StackExchange.Redis;

  public class RedisEventBus : InMemoryEventBus, IEventPublisher
  {
    private string toptic = "events";
    private readonly ConnectionMultiplexer redis;
    private readonly ISubscriber subscriber;
    private readonly JsonSerializerSettings settings;

    public RedisEventBus(ConnectionMultiplexer redis)
    {
      this.settings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.Auto
      };
      this.redis = redis;
      this.subscriber = redis.GetSubscriber();

      this.subscriber.Subscribe(toptic, this.OnMessage);
    }

    public override Task PublishAsync<TEvent>(TEvent @event)
    {
      var desc = new EventDescriptor(@event);
      return this.subscriber.PublishAsync(toptic, JsonConvert.SerializeObject(desc, this.settings));
    }

    protected async virtual void OnMessage(RedisChannel channel, RedisValue message)
    {
      try
      {
        var desc = JsonConvert.DeserializeObject<EventDescriptor>((string)message, this.settings);
        await this.DispatchAsync((IEvent)desc.Data);
      }
      catch (Exception)
      {
        // nothing
      }
    }
  }
}