using System;
using System.Threading.Tasks;
using System.Linq;
using DDDLite.CQRS;
using DDDLite.CQRS.Events;
using DDDLite.CQRS.Messaging.Kafka;

namespace KafkaEventTest
{
  public class Test : EventSource { }
  public class TestCreated : Event
  {
    public string Message { get; set; }
  }

  class Program
  {
    private static KafkaEventBus eventBus;

    static void Main(string[] args)
    {
      Console.Write("Ready?");
      Console.ReadLine();
      eventBus = new KafkaEventBus(new KafkaEventBusOptions
      {
        Host = "localhost",
        GroupId = "demo",
        PublishToptic = "demo"
      }, null);
      
    var cts = new System.Threading.CancellationTokenSource();

    var task = Task.Factory.StartNew(() =>
    {
        try
        {
            eventBus.Listening(cts.Token);
        }
        catch { }

        eventBus.Dispose();
    });

      RegisterHandlers();
      PublishEvents();

      Console.WriteLine("Waiting!");
      Console.ReadLine();
      cts.Cancel();
    }

    static void RegisterHandlers()
    {
      eventBus.RegisterHandler<TestCreated>((@event) =>
      {
        Console.WriteLine("Task 1: {0}", @event.Message);
        return Task.CompletedTask;
      });

      eventBus.RegisterHandler<TestCreated>((@event) =>
      {
        Console.WriteLine("Task 2: {0}", @event.Message);
        return Task.CompletedTask;
      });

      eventBus.RegisterHandler<TestCreated>((@event) =>
      {
        // throw new ArgumentException("evnet");
        Console.WriteLine("Task 3: {0}", @event.Message);
        return Task.CompletedTask;
      });
    }

    static void PublishEvents()
    {
      var events = new TestCreated[]
      {
        new TestCreated { Message = "Hello, World!" },
        new TestCreated { Message = "Hello, Redis!" }
      };

      events.All((e) =>
      {
        eventBus.PublishAsync(e);
        return true;
      });
    }
  }
}
