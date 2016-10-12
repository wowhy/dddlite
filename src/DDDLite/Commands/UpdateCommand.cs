namespace DDDLite.Commands
{
    using Domain;

    public class UpdateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}