namespace DDDLite.Commands
{
    public class CreateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}