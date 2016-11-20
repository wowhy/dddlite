namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    using Events;

    public class EventConsumer : DisposableObject, IEventConsumer
    {
        private bool disposed;

        public IMessageSubscriber Subscriber { get; protected set; }

        private readonly IEventHandlerFactory factory;

        public EventConsumer(IMessageSubscriber subscriber, IEventHandlerFactory factory)
        {
            this.Subscriber = subscriber;
            this.factory = factory;


            subscriber.MessageReceived += (sender, e) =>
            {
                var messageType = e.Message.GetType();
                foreach (var handler in this.factory.GetEventHandlers(messageType))
                {
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