namespace DDDLite.Commands
{
    public class DeleteCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}