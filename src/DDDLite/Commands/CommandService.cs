namespace DDDLite.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    using Common;
    using Messaging;

    public class CommandService : DisposableObject, ICommandService
    {
        public readonly static Type CommandHandlerType = typeof(ICommandHandler<>);

        public readonly static ConcurrentDictionary<Type, Tuple<Type, MethodInfo>> HandlerTypeCache = new ConcurrentDictionary<Type, Tuple<Type, MethodInfo>>();


        private bool disposed;


        public IMessageSubscriber Subscriber { get; protected set; }

        public IServiceProvider ServiceProvider { get; protected set; }

        public CommandService(IMessageSubscriber subscriber, IServiceProvider serviceProvider)
        {
            this.Subscriber = subscriber;
            this.ServiceProvider = serviceProvider;

            subscriber.MessageReceived += (sender, e) =>
            {
                if (this.ServiceProvider == null)
                {
                    return;
                }

                var messageType = e.Message.GetType();
                var handlerType = HandlerTypeCache.GetOrAdd(messageType, type =>
                {
                    var baseType = type;
                    if (type.GetTypeInfo().GetInterface("ICreateCommand`1") != null)
                    {
                        baseType = type.GetTypeInfo().GetInterface("ICreateCommand`1");
                    }

                    if (type.GetTypeInfo().GetInterface("IUpdateCommand`1") != null)
                    {
                        baseType = type.GetTypeInfo().GetInterface("IUpdateCommand`1");
                    }

                    if (type.GetTypeInfo().GetInterface("IDeleteCommand`1") != null)
                    {
                        baseType = type.GetTypeInfo().GetInterface("IDeleteCommand`1");
                    }

                    var _type = CommandHandlerType.MakeGenericType(baseType);
                    return new Tuple<Type, MethodInfo>(
                        _type,
                        _type.GetTypeInfo().GetInterface("IHandler`1").GetTypeInfo().GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    );
                });

                var handler = this.ServiceProvider.GetService(handlerType.Item1);

                if (handler == null)
                {
                    return;
                }

                if (handlerType.Item2 == null)
                {
                    return;
                }

                try
                {
                    handlerType.Item2.Invoke(handler, new[] { e.Message });
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.Subscriber.Dispose();
                    disposed = true;
                }
            }
        }
    }
}
