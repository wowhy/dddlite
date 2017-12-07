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
  using Microsoft.Extensions.Logging;

  using DDDLite.CQRS.Events;

  using InMemory;
  using System.Threading;

  public class KafkaEventBus : InMemoryEventBus, IEventPublisher, IDisposable
  {
    private readonly KafkaEventBusOptions options;
    private Producer<Null, IEvent> producer;
    private Consumer<Null, IEvent> consumer;

    public KafkaEventBus(KafkaEventBusOptions options, ILogger<KafkaEventBus> logger)
    {
      this.options = options;
      this.Logger = logger;

      this.producer = new Producer<Null, IEvent>(new Dictionary<string, object>
      {
        { "bootstrap.servers", options.Host }
      }, null, new EventSerializer());

      this.consumer = new Consumer<Null, IEvent>(new Dictionary<string, object>
      {
        { "bootstrap.servers", options.Host },
        { "group.id", options.GroupId }
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
      await this.producer.ProduceAsync(options.PublishToptic, null, @event);
    }

    public void Listening(CancellationToken cancellationToken)
    {
      var toptics = new List<string> { options.PublishToptic };
      if (options.SubscribeToptics != null)
      {
        toptics.AddRange(options.SubscribeToptics);
      }

      this.consumer.Subscribe(toptics.Distinct().ToArray());

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
      catch (Exception ex)
      {
        this.Logger?.LogError(0, ex, ex.Message);
      }
    }
  }
}