namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using Events;

    public class EventConsumer : DisposableObject, IEventConsumer
    {
        private bool disposed;

        public IMessageSubscriber Subscriber { get; protected set; }

        public IServiceProvider ServiceProvider { get; protected set; }

        private readonly Dictionary<Type, Func<IServiceProvider, IEventHandler>> handlerCreators;

        public EventConsumer(IMessageSubscriber subscriber, IServiceProvider serviceProvider, IEnumerable<KeyValuePair<Type, Func<IServiceProvider, IEventHandler>>> handlerCreators)
        {
            this.Subscriber = subscriber;
            this.ServiceProvider = serviceProvider;
            this.handlerCreators = new Dictionary<Type, Func<IServiceProvider, IEventHandler>>();

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