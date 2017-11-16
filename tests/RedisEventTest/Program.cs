using System;
using System.Threading.Tasks;
using System.Linq;
using DDDLite.CQRS;
using DDDLite.CQRS.Events;
using DDDLite.CQRS.Messaging.Redis;
using StackExchange.Redis;

namespace RedisEventTest
{
  public class Test : EventSource { }
  public class TestCreated : Event
  {
    public string Message { get; set; }
  }

  class Program
  {
    private static ConnectionMultiplexer redis;
    private static RedisEventBus eventBus;

    static void Main(string[] args)
    {
      Console.Write("Ready?");
      Console.ReadLine();
      redis = ConnectionMultiplexer.Connect("localhost");
      eventBus = new RedisEventBus(redis);

      redis.PreserveAsyncOrder = false;

      RegisterHandlers();
      PublishEvents();

      Console.WriteLine("Waiting!");
      // var sub = redis.GetSubscriber();
      // sub.Subscribe("messages", (channel, message) =>
      // {
      //   Console.WriteLine(message);
      // });

      // sub.Publish("messages", "hello");

      Console.ReadLine();
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
