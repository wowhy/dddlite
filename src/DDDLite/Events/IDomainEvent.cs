namespace DDDLite.Events
{
    using System;

    public interface IDomainEvent : IMessage
    {
        Guid AggregateRootId { get; set; }

        object AggregateRoot { get; set; }

        string AggregateRootType { get; set; }

        string EventName { get; set; }
    }

    public interface IDomainEvent<TAggregateRoot> : IDomainEvent
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
