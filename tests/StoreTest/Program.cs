using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDLite.CQRS;
using DDDLite.CQRS.Events;
using DDDLite.CQRS.Npgsql;

namespace StoreTest
{
  public class Order : EventSource
  { }

  public class OrderCreated : Event
  {
    public string Product { get; set; }

    public int Counts { get; set; }

    public decimal TotalPrice { get; set; }
  }

  public class OrderPaymented : Event
  {
    public DateTime PaymentTime { get; set; }

    public decimal PaymentMoney { get; set; }
  }

  public class OrderCanceled : Event
  {
    public string Memo { get; set; }
  }

  class Program
  {
    static string connectionString = "Server=127.0.0.1;Port=5432;Database=store_test;User Id=postgres;Password=hongyuan;";
    static Guid order1 = Guid.Parse("39e29467-1865-42ee-98d3-c6ad5b875ed4");
    static Guid order2 = Guid.Parse("39e29467-1866-4e3a-09c8-00503cf9de40");
    static void Main(string[] args)
    {
      // TestWrite().Wait();
      TestRead().Wait();
    }

    async static Task TestRead()
    {
      var eventStore = new NpgsqlEventStore(connectionString);
      var events = await eventStore.GetAsync<Order>(order2, -1);

      foreach (var e in events)
      {
        Console.WriteLine($"Id: {e.Id}, AggregateRootId: {e.AggregateRootId}, Type: {e.GetType().Name}");
      }
    }

    async static Task TestWrite()
    {
      var eventStore = new NpgsqlEventStore(connectionString);
      await eventStore.SaveAsync<Order>(GetEvents());
    }

    static IEnumerable<IEvent> GetEvents()
    {

      return new List<IEvent>
      {
        new OrderCreated
        {
          AggregateRootId = order1,
          Product = "test1",
          Counts = 10,
          TotalPrice = 1024,
          RowVersion = 0
        },
        new OrderCreated
        {
          AggregateRootId = order2,
          Product = "test1",
          Counts = 10,
          TotalPrice = 1024,
          RowVersion = 0
        },
        new OrderCanceled
        {
          AggregateRootId = order1,
          Memo = "Hello, World!",
          RowVersion = 1
        },
        new OrderPaymented
        {
          AggregateRootId = order2,
          PaymentMoney = 100,
          PaymentTime = DateTime.Now,
          RowVersion = 1
        },
        new OrderCanceled
        {
          AggregateRootId = order2,
          Memo = "!!!!!!!!!!!!!",
          RowVersion = 2
        }
      };
    }
  }
}
