namespace DDDLite.CQRS.Messaging.Redis
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Events;
  using Newtonsoft.Json;
  using StackExchange.Redis;
  using FS = Foundatio.Messaging;

  public class RedisEventBus : IEventPublisher
  {
    private readonly ConnectionMultiplexer redis;
    private readonly ISubscriber subscriber;
    private readonly FS.RedisMessageBus innerBus;

    public RedisEventBus(ConnectionMultiplexer redis)
    {
      this.redis = redis;
      this.subscriber = redis.GetSubscriber();
      this.innerBus = new FS.RedisMessageBus(new FS.RedisMessageBusOptions
      {
        Subscriber = this.subscriber
      });
    }

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
      return this.innerBus.PublishAsync(typeof(TEvent), JsonConvert.SerializeObject(@event));
    }

    public void RegisterHandler<TEvent>(Func<IEvent, Task> handler) where TEvent : class, IEvent
    {
      this.innerBus.SubscribeAsync<TEvent>((@event, cancellationToken) =>
      {
        return handler(@event);
      });
    }
  }
}