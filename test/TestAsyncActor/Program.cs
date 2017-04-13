using Akka;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.Routing;
using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Greet
{
    public int Id { get; set; }

    public Greet(string who)
    {
        Who = who;
    }
    public string Who { get; private set; }
}

public class GreetingActor : ReceiveActor
{
    public GreetingActor()
    {
        //this.ReceiveAsync<Greet>(this.Handle);
        this.Receive<int>(i => this.Handle(i));
    }

    public void Handle(int i)
    {
        Console.WriteLine($"Hello, {i} of {Self.Path}");
    }

    public async Task Handle(Greet greet)
    {
        await TestAsync("先等一会儿");
        Console.WriteLine("Hello {0}!", greet.Who);
        await TestAsync("再等一会儿");
        Console.WriteLine("结束了");
        Sender.Tell(greet.Id);
    }

    public Task TestAsync(string message)
    {
        Console.WriteLine(message);
        return Task.Delay(1000);
    }
}

class Program
{
    static void Main(string[] args)
    {
        RunProgram();
    }

    public static void RunProgram()
    {
        var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
        serializers {
            wire = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
        }
        serialization-bindings {
            ""System.Object"" = wire
        }
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            port = 8081
            hostname = localhost
        }
    }
}
        ");
        var builder = new ContainerBuilder();
        builder.RegisterType<GreetingActor>().AsSelf();
        var container = builder.Build();
        using (var system = ActorSystem.Create("TestSystem", config))
        {
            var resolver = new AutoFacDependencyResolver(container, system);
            var props = resolver.Create<GreetingActor>().WithRouter(new RoundRobinPool(5));

            var actor = system.ActorOf(props, "greeters");
            Console.WriteLine(actor.Path);

            Console.ReadLine();
        }
    }
}