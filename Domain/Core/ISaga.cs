namespace Domain.Core
{
    using System;
    using Events;

    public interface ISaga<TMessage> : IAggregateRoot
        where TMessage : DomainEvent
    {
        void Transit(TMessage message);
    }
}
