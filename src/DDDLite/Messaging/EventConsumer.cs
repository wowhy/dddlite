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

        private readonly IDictionary<Type, Func<IEventHandler>> eventRoutes;

        public EventConsumer(IMessageSubscriber subscriber, IEnumerable<KeyValuePair<Type, Func<IEventHandler>>> handlerCreators)
        {
            this.Subscriber = subscriber;
            this.eventRoutes = new Dictionary<Type, Func<IEventHandler>>();
            foreach (var ctor in handlerCreators)
            {
                var typeInfo = ctor.Key.GetTypeInfo();
                var eventTypes = typeInfo.GetInterfaces()
                    .Where(k => k.IsConstructedGenericType && k.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    .Select(k => k.GenericTypeArguments[0]);

                foreach (var eventType in eventTypes)
                {
                    this.eventRoutes.Add(eventType, ctor.Value);
                }
            }

            subscriber.MessageReceived += (sender, e) =>
            {
                var messageType = e.Message.GetType();
                if (this.eventRoutes.ContainsKey(messageType))
                {
                    var creator = this.eventRoutes[messageType];
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