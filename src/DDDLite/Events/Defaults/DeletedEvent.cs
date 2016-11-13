namespace DDDLite.Events
{
    public class DeletedEvent<TAggregateRoot> : DomainEvent<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
