namespace DDDLite.Messaging
{
    using System;
    using System.Collections.Generic;

    using Events;

    public interface IEventHandlerFactory
    {
        IEnumerable<IEventHandler> GetEventHandlers(Type eventType);
    }
}
