namespace DDDLite.Commands
{
    using Domain;

    public class CreateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}