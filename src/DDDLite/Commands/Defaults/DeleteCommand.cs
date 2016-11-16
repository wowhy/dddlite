namespace DDDLite.Commands
{
    public class DeleteCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}