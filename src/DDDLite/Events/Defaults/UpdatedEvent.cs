namespace DDDLite.Events
{
    public class UpdatedEvent<TAggregateRoot> : DomainEvent<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
