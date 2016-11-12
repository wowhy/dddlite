namespace DDDLite.Events
{
    using System;

    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set;}

        Object AggregateRoot { get; set; }
    }
}
