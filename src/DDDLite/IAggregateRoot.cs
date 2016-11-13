namespace DDDLite
{
    using System;
    using System.Collections.Generic;
    using Events;

    public interface IAggregateRoot : IEntity
    {
        long RowVersion { get; set; }

        Guid? CreatedById { get; set; }

        DateTime CreatedOn { get; set; }

        Guid? ModifiedById { get; set; }

        DateTime? ModifiedOn { get; set; }

        IEnumerable<IDomainEvent> UncommittedEvents { get; }

        void RaiseEvent<TEvent>(TEvent e) where TEvent : class, IDomainEvent;
    }
}
