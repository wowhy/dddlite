namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Reflection;

    using Commands;

    public sealed class CommandConsumer : DisposableObject, ICommandConsumer
    {
        private bool disposed;

        public IMessageSubscriber Subscriber { get; private set; }

        private readonly Dictionary<Type, Func<ICommandHandler>> handlerCreators;

        public CommandConsumer(IMessageSubscriber subscriber, IEnumerable<KeyValuePair<Type, Func<ICommandHandler>>> handlerCreators)
        {
            this.Subscriber = subscriber;
            this.handlerCreators = new Dictionary<Type, Func<ICommandHandler>>();

            if (handlerCreators != null)
            {
                foreach (var creator in handlerCreators)
                {
                    this.handlerCreators[creator.Key] = creator.Value;
                }
            }

            subscriber.MessageReceived += (sender, e) =>
            {
                var messageType = e.Message.GetType();
                if (this.handlerCreators.ContainsKey(messageType))
                {
                    var creator = this.handlerCreators[messageType];
                    var handler = creator();
                    if (handler != null)
                    {
                        var method = handler.GetType().GetTypeInfo().GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
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
                            throw new CoreException("命令无法找到符合条件的方法！");
                        }
                    }
                    else
                    {
                        throw new CoreException("无法找到相关命令！");
                    }
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
