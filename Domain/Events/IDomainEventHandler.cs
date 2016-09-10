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

    public interface IDomainEventHandler<TEntityKey, TEvent> : IHandler<TEvent>, IDomainEventHandler
        where TEvent : class, IDomainEvent<TEntityKey>
        where TEntityKey : IEquatable<TEntityKey>
    {
    }
}
