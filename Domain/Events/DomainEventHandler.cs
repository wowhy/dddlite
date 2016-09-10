namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<Guid, TEvent>
        where TEvent : DomainEvent
    {
        protected DomainEventHandler()
        {
        }

        public abstract void Handle(TEvent message);

        public abstract Task HandleAsync(TEvent message);
    }
}
