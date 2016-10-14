namespace DDDLite.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Threading.Tasks;

    using Common;
    using Messaging;

    public class CommandService : DisposableObject, ICommandService
    {
        public readonly static Type CommandHandlerType = typeof(ICommandHandler<>);

        private ConcurrentDictionary<Type, Tuple<Type, MethodInfo>> handlers = new ConcurrentDictionary<Type, Tuple<Type, MethodInfo>>();

        public IMessageSubscriber Subscriber { get; protected set; }

        public IServiceProvider ServiceProvider { get; protected set; }

        public CommandService(IMessageSubscriber subscriber, IServiceProvider serviceProvider)
        {
            this.Subscriber = subscriber;
            this.ServiceProvider = serviceProvider;

            subscriber.MessageReceived += async (sender, e) =>
            {
                if (this.ServiceProvider == null)
                {
                    return;
                }

                var messageType = e.Message.GetType();
                var handlerType = handlers.GetOrAdd(messageType, type =>
                {
                    var _type = CommandHandlerType.MakeGenericType(type);
                    return new Tuple<Type, MethodInfo>(
                        _type,
                        _type.GetTypeInfo().GetMethod("HandleAsync")
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

                await (Task)handlerType.Item2.Invoke(handler, new[] { e.Message });
            };
        }
    }
}
