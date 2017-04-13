using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TestActorClient
{
    public class Program
    {
        public static void Main(string[] args)
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
            port = 0
            hostname = localhost
        }
    }
}
        ");
            using (var system = ActorSystem.Create("TestSystem-Client", config))
            {
                var actor = system.ActorSelection("akka.tcp://TestSystem@localhost:8081/user/greeters");

                for (var i = 0; i < 10; i++)
                {
                    actor.Tell(i);
                }
                
                Console.ReadLine();
            }
        }
    }
}
