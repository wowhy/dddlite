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

        public IServiceProvider ServiceProvider { get; private set; }

        private readonly Dictionary<Type, Func<IServiceProvider, ICommandHandler>> handlerCreators;

        public CommandConsumer(IMessageSubscriber subscriber, IServiceProvider serviceProvider, IEnumerable<KeyValuePair<Type, Func<IServiceProvider, ICommandHandler>>> handlerCreators)
        {
            this.Subscriber = subscriber;
            this.ServiceProvider = serviceProvider;
            this.handlerCreators = new Dictionary<Type, Func<IServiceProvider, ICommandHandler>>();

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
                    var handler = creator(this.ServiceProvider);
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
