namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<TEvent> : IHandler<TEvent>, IDomainEventHandler
        where TEvent : class, IDomainEvent
    {
    }
}
