namespace DDDLite.Config
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using Commands;
    using Messaging;
    using System.Collections.Generic;
    using Events;

    public abstract class Register
    {
        private readonly IServiceCollection services;

        protected Register(IServiceCollection services)
        {
            this.services = services;
        }

        public virtual void Load(params Assembly[] assemblies)
        {
            this.RegisterCommandBus();
            this.RegisterEventBus();
            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    this.RegisterCommandHandlers(assembly);
                    this.RegisterQueryServices(assembly);
                }

                this.RegisterAutoMapper(assemblies);
            }
        }

        protected virtual IDictionary<Type, Func<IServiceProvider, ICommandHandler>> CommandHandlers => new Dictionary<Type, Func<IServiceProvider, ICommandHandler>>();
        protected virtual IDictionary<Type, Func<IServiceProvider, IEventHandler>> EventHandlers => new Dictionary<Type, Func<IServiceProvider, IEventHandler>>();

        protected virtual void RegisterCommandBus()
        {
            // register command sender
            services.AddSingleton<InProcessCommandBus>();
            services.AddSingleton<ICommandSender>(provider => provider.GetService<InProcessCommandBus>());
            services.AddSingleton<ICommandConsumer>(provider =>
            {
                var ctors = from p in this.CommandHandlers
                            select new KeyValuePair<Type, Func<ICommandHandler>>(p.Key, () => p.Value(provider));
                return new CommandConsumer(provider.GetService<InProcessCommandBus>(), ctors);
            });
        }

        protected virtual void RegisterEventBus()
        {
            // register event sender
            services.AddSingleton<InProcessEventBus>();
            services.AddSingleton<IEventPublisher>(provider => provider.GetService<InProcessEventBus>());
            services.AddSingleton<IEventConsumer>(provider =>
            {
                var ctors = from p in this.EventHandlers
                            select new KeyValuePair<Type, Func<IEventHandler>>(p.Key, () => p.Value(provider));
                return new EventConsumer(provider.GetService<InProcessEventBus>(), ctors);
            });
        }

        protected virtual void RegisterCommandHandlers(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var commandHandlerTypes = types.Where(k => k.Name.EndsWith("CommandHandler") && k.GetTypeInfo().IsClass);

            Console.WriteLine("Register CommandHandlers");
            foreach (var type in commandHandlerTypes)
            {
                this.services.AddScoped(type);
            }
            Console.WriteLine();
        }

        protected virtual void RegisterQueryServices(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var queryServiceTypes = types.Where(k => k.Name.EndsWith("QueryService") && k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);

            Console.WriteLine("Register Querying Services");
            foreach (var type in queryServiceTypes)
            {
                Console.WriteLine("type: " + type.FullName);
                var typeInfo = type.GetTypeInfo();
                var interfaceType = typeInfo.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    this.services.AddScoped(type);
                }
                else
                {
                    this.services.AddScoped(interfaceType, type);
                }
            }
            Console.WriteLine();
        }

        protected virtual void RegisterAutoMapper(params Assembly[] assemblies)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(assemblies);
            });
        }
    }
}
