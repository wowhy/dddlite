namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Linq;

    using Commands;

    public sealed class CommandConsumer : DisposableObject, ICommandConsumer
    {
        private bool disposed;

        public IMessageSubscriber Subscriber { get; private set; }

        private readonly IDictionary<Type, Func<ICommandHandler>> commandRoutes;

        public CommandConsumer(IMessageSubscriber subscriber, IEnumerable<KeyValuePair<Type, Func<ICommandHandler>>> handlerCreators)
        {
            this.Subscriber = subscriber;
            this.commandRoutes = new Dictionary<Type, Func<ICommandHandler>>();
            foreach (var ctor in handlerCreators)
            {
                var typeInfo = ctor.Key.GetTypeInfo();
                var commandTypes = typeInfo.GetInterfaces()
                    .Where(k => k.IsConstructedGenericType && k.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                    .Select(k => k.GenericTypeArguments[0]);

                foreach (var commandType in commandTypes)
                {
                    this.commandRoutes.Add(commandType, ctor.Value);
                }
            }

            if (this.commandRoutes != null)
            {
                foreach (var ctor in handlerCreators)
                {
                    this.commandRoutes.Add(ctor.Key, ctor.Value);
                }
            }

            subscriber.MessageReceived += (sender, e) =>
            {
                var messageType = e.Message.GetType();
                if (this.commandRoutes.ContainsKey(messageType))
                {
                    var creator = this.commandRoutes[messageType];
                    var handler = creator();
                    if (handler != null)
                    {
                        var method = handler.GetType().GetRuntimeMethod("Handle", new Type[] { messageType });
                        if (method != null)
                        {
                            try
                            {
                                method.Invoke(handler, new[] { e.Message });
                            }
                            catch (Exception ex)
                            {
                                throw ex.InnerException;
                            }
                        }
                        else
                        {
                            throw new CoreException("命令处理程序缺少Handle方法！");
                        }
                    }
                    else
                    {
                        throw new CoreException("无法初始化命令处理程序！");
                    }
                }
                else
                {
                    throw new CoreException("无法找到相关命令处理程序！");
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
