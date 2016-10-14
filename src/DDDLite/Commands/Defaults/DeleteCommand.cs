namespace DDDLite.Commands
{
    using Domain;

    public class DeleteCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}