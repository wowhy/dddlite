namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading.Tasks;

  using Confluent.Kafka;
  using Confluent.Kafka.Serialization;
  using Newtonsoft.Json;

  using DDDLite.CQRS.Events;

  using InMemory;
  using System.Threading;

  public class KafkaEventBus : InMemoryEventBus, IEventPublisher, IDisposable
  {
    private readonly string topic;
    private Producer<Null, string> producer;
    private Consumer<Null, string> consumer;
    private readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
      TypeNameHandling = TypeNameHandling.Auto
    };

    public KafkaEventBus(string host, string groupId, string topic)
    {
      this.topic = topic;
      this.producer = new Producer<Null, string>(new Dictionary<string, object>
      {
        { "bootstrap.servers", host }
      }, null, new StringSerializer(Encoding.UTF8));

      this.consumer = new Consumer<Null, string>(new Dictionary<string, object>
      {
        { "bootstrap.servers", host },
        { "group.id", groupId }
      }, null, new StringDeserializer(Encoding.UTF8));
      this.consumer.Subscribe(topic);
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
      var desc = new EventDescriptor(@event);
      await this.producer.ProduceAsync(topic, null, JsonConvert.SerializeObject(desc, this.settings));
    }

    public void Listening(CancellationToken cancellationToken, params string[] topics)
    {
      if (topics != null && topics.Length > 0)
      {
        this.consumer.Subscribe(topics);
      }

      while (true)
      {
        cancellationToken.ThrowIfCancellationRequested();
        Message<Null, string> msg;
        if (!consumer.Consume(out msg, TimeSpan.FromMilliseconds(100)))
        {
          continue;
        }

        cancellationToken.ThrowIfCancellationRequested();

        OnMessage(msg).Wait();
        consumer.CommitAsync().Wait();
      }
    }

    protected async Task OnMessage(Message<Null, string> e)
    {
      try
      {
        var message = e.Value;
        var desc = JsonConvert.DeserializeObject<EventDescriptor>(message, this.settings);
        await this.DispatchAsync((IEvent)desc.Data);
      }
      catch (Exception)
      {
        // nothing
      }
    }
  }
}