using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDLite.CQRS;
using DDDLite.CQRS.Events;
using DDDLite.CQRS.Store.Npgsql;
using DDDLite.CQRS.Snapshots;
using Newtonsoft.Json;

namespace StoreTest
{
  public class Order : SnapshotEventSource<OrderSnapshot>
  {
    public string Product { get; set; }
    public string Memo { get; set; }

    public string Status { get; set; }

    public override OrderSnapshot GetSnapshot()
    {
      var snapshot = new OrderSnapshot
      {
        Id = this.Id,
        Version = this.Version,
        Product = this.Product
      };
      return snapshot;
    }

    public override void RestoreFromSnapshot(OrderSnapshot snapshot)
    {
      this.Id = snapshot.Id;
      this.Version = snapshot.Version;
      this.Product = snapshot.Product;
    }
  }

  public class OrderSnapshot : Snapshot
  {
    public string Product { get; set; }
  }

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
      TestWrite().Wait();
      TestRead().Wait();
      TestSnapshots().Wait();
    }

    async static Task TestSnapshots()
    {
      var store = new NpgsqlSnapshotStore(connectionString);
      var snapshot = await store.GetAsync<OrderSnapshot>(order1);

      if (snapshot == null) 
      {
        await store.SaveAsync(new OrderSnapshot
        {
          Id = order1,
          Version = 1,
          Product = "test"
        });
      } else 
      {
        Console.WriteLine("Find snapshot: ");
        Console.WriteLine(JsonConvert.SerializeObject(snapshot));
      }
    }

    async static Task TestRead()
    {
      var eventStore = new NpgsqlEventStore(connectionString);
      var events = await eventStore.GetAsync<Order>(order2, -1);

      foreach (var e in events)
      {
        Console.WriteLine($"Id: {e.Id}, Type: {e.GetType().Name}");
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
          Id = order1,
          Product = "test1",
          Counts = 10,
          TotalPrice = 1024,
          Version = 0
        },
        new OrderCreated
        {
          Id = order2,
          Product = "test1",
          Counts = 10,
          TotalPrice = 1024,
          Version = 0
        },
        new OrderCanceled
        {
          Id = order1,
          Memo = "Hello, World!",
          Version = 1
        },
        new OrderPaymented
        {
          Id = order2,
          PaymentMoney = 100,
          PaymentTime = DateTime.Now,
          Version = 1
        },
        new OrderCanceled
        {
          Id = order2,
          Memo = "!!!!!!!!!!!!!",
          Version = 2
        }
      };
    }
  }
}
