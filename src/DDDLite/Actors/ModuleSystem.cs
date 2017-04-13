namespace DDDLite.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Autofac;

    using Akka;
    using Akka.Actor;
    using Akka.Event;
    using Akka.DI.Core;

    public abstract class ModuleSystem
    {
        public IActorRefFactory ActorFactory { get; protected set; }

        public IDependencyResolver ActorResolver { get; protected set; }

        public IContainer Container { get; protected set; }

        protected ModuleSystem(IActorRefFactory actorSystem, IDependencyResolver resolver, IContainer container)
        {
            this.ActorFactory = actorSystem;
            this.ActorResolver = resolver;
            this.Container = container;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Shotdown()
        {
            var system = ActorFactory as ActorSystem;
            if (system != null)
            {
                system.Terminate().Wait();
            }
        }

        public ICanTell GetActor(string path)
        {
            return this.ActorFactory.ActorSelection(path);
        }

        public ICanTell GetActor(ActorMetaData meta)
        {
            return this.ActorFactory.ActorSelection(meta.Path);
        }


        public virtual T Resolve<T>()
        {
            return this.Container.Resolve<T>();
        }
    }
}
