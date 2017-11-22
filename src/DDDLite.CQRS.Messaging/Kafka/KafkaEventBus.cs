namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading.Tasks;
  using System.Linq;

  using Confluent.Kafka;
  using Confluent.Kafka.Serialization;
  using Newtonsoft.Json;

  using DDDLite.CQRS.Events;

  using InMemory;
  using System.Threading;

  public class KafkaEventBus : InMemoryEventBus, IEventPublisher, IDisposable
  {
    private readonly string topic;
    private Producer<Null, IEvent> producer;
    private Consumer<Null, IEvent> consumer;

    public KafkaEventBus(string host, string groupId, string topic)
    {
      this.topic = topic;
      this.producer = new Producer<Null, IEvent>(new Dictionary<string, object>
      {
        { "bootstrap.servers", host }
      }, null, new EventSerializer());

      this.consumer = new Consumer<Null, IEvent>(new Dictionary<string, object>
      {
        { "bootstrap.servers", host },
        { "group.id", groupId }
      }, null, new EventDeserializer());
    }
    public void Dispose()
    {
      if (this.consumer != null)
      {
        this.consumer.Dispose();
        this.consumer = null;
      }

      if (this.producer != null)
      {
        this.producer.Flush(100);
        this.producer.Dispose();
        this.producer = null;
      }
    }

    public async override Task PublishAsync<TEvent>(TEvent @event)
    {
      await this.producer.ProduceAsync(topic, null, @event);
    }

    public void Listening(CancellationToken cancellationToken, params string[] topics)
    {
      var tmp = new List<string> { topic };
      if (topics != null)
      {
        tmp.AddRange(topics);
      }

      this.consumer.Subscribe(tmp.Distinct().ToArray());

      while (true)
      {
        cancellationToken.ThrowIfCancellationRequested();
        Message<Null, IEvent> msg;
        if (!consumer.Consume(out msg, TimeSpan.FromMilliseconds(100)))
        {
          continue;
        }

        cancellationToken.ThrowIfCancellationRequested();

        OnMessage(msg).Wait();
        consumer.CommitAsync().Wait();
      }
    }

    protected async Task OnMessage(Message<Null, IEvent> e)
    {
      try
      {
        await this.DispatchAsync(e.Value);
      }
      catch (Exception)
      {
        // nothing
      }
    }
  }
}