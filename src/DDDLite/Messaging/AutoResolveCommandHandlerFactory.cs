namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    using Commands;

    public class AutoResolveCommandHandlerFactory : ICommandHandlerFactory
    {
        private static Dictionary<Type, Type> routes = new Dictionary<Type, Type>();

        static AutoResolveCommandHandlerFactory()
        {
            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            foreach (var assemblyName in assemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                var types = assembly.GetExportedTypes().Where(k => k.GetTypeInfo().IsClass && !k.GetTypeInfo().IsAbstract);
                foreach (var type in types)
                {
                    var commandTypes = type.GetTypeInfo().GetInterfaces()
                        .Where(k => k.IsConstructedGenericType && k.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                        .Select(k => k.GenericTypeArguments[0]);

                    foreach (var commandType in commandTypes)
                    {
                        routes.Add(commandType, type);
                    }
                }
            }
        }

        private readonly IServiceProvider serviceProvider;

        public AutoResolveCommandHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICommandHandler<T> GetHandler<T>() where T : class, ICommand
        {
            return (ICommandHandler<T>)this.GetHandler(typeof(T));
        }

        public ICommandHandler GetHandler(Type commandType)
        {
            if (!routes.ContainsKey(commandType))
            {
                throw new CoreException("无法找到相关命令处理程序！");
            }

            return (ICommandHandler)this.serviceProvider.GetService(routes[commandType]);
        }
    }
}
