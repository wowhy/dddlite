namespace DDDLite.CQRS.Messaging.Redis
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Events;
  using Newtonsoft.Json;
  using StackExchange.Redis;

  public class RedisEventBus : IEventPublisher
  {
    private string toptic = "events";
    private readonly ConnectionMultiplexer redis;
    private readonly ISubscriber subscriber;

    public RedisEventBus(ConnectionMultiplexer redis)
    {
      this.redis = redis;
      this.subscriber = redis.GetSubscriber();
    }

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
      return this.subscriber.PublishAsync(toptic, JsonConvert.SerializeObject(@event));
    }

    public void RegisterHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : class, IEvent
    {
      this.subscriber.Subscribe(toptic, async (chanel, message) =>
      {
        try
        {
          var @event = (TEvent)JsonConvert.DeserializeObject((string)message, typeof(TEvent));
          await handler(@event);
        }
        catch (Exception)
        {
        }
      });
    }
  }
}