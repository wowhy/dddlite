namespace DDDLite.Commands
{
    public class CreateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}