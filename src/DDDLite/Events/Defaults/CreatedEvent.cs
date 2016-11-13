namespace DDDLite.Events
{
    public class CreatedEvent<TAggregateRoot> : DomainEvent<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
