namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Events;

    public class AutoResolveEventHandlerFactory : IEventHandlerFactory
    {
        private static Dictionary<Type, List<Type>> routes = new Dictionary<Type, List<Type>>();

        static AutoResolveEventHandlerFactory()
        {
            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            foreach (var assemblyName in assemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                var types = assembly.GetExportedTypes().Where(k => k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);
                foreach (var type in types)
                {
                    var eventTypes = type.GetTypeInfo().GetInterfaces()
                        .Where(k => k.IsConstructedGenericType && k.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                        .Select(k => k.GenericTypeArguments[0]);

                    foreach (var eventType in eventTypes)
                    {
                        // routes.Add(eventType, type);
                        var list = routes[eventType];
                        if (list == null)
                        {
                            list = routes[eventType] = new List<Type>();
                        }

                        list.Add(type);
                    }
                }
            }
        }

        private readonly IServiceProvider serviceProvider;

        public AutoResolveEventHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(Type eventType)
        {
            if (!routes.ContainsKey(eventType))
            {
                return new List<IEventHandler>();
            }

            return routes[eventType].Select(k => (IEventHandler)this.serviceProvider.GetService(k));
        }
    }
}
