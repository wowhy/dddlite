using DDDLite.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Autofac;
using Example.Core.Actors;
using Akka.DI.AutoFac;

namespace Example.Actors
{
    public sealed class ExampleSystem : ModuleSystem
    {
        public static ExampleSystem Context { get; private set; }

        private ExampleSystem(IActorRefFactory actorSystem, IDependencyResolver resolver, IContainer container) :
            base(actorSystem, resolver, container)
        {
        }

        public override void Initialize()
        {
            this.InitCommandActors();
            this.InitQueryActors();
        }

        private void InitCommandActors()
        {
            ActorFactory.ActorOf(ActorResolver.Create<UserCommandActor>(), ActorPaths.UserCommand.Name);

            // ActorFactory.ActorOf(ActorResolver.Create<CommandActor<Demo>>(), ActorPaths.DemoCommand.Name);
        }

        private void InitQueryActors()
        {
            ActorFactory.ActorOf(ActorResolver.Create<QueryActor<Core.Querying.User>>(), ActorPaths.UserQuery.Name);

            // ActorFactory.ActorOf(ActorResolver.Create<QueryActor<Demo>>(), ActorPaths.DemoQuery.Name);
        }

        public static void RegisterActorTypes(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(QueryActor<>))
                    .AsSelf();

            builder.RegisterGeneric(typeof(CommandActor<>))
                   .AsSelf();

            builder.RegisterType<UserCommandActor>().AsSelf();
            builder.RegisterType<UserValidateActor>().AsSelf();
        }

        public static ExampleSystem Create(IActorRefFactory actorSystem, IDependencyResolver resolver, IContainer container)
        {
            Context = new ExampleSystem(actorSystem, resolver, container);
            return Context;
        }
    }
}
