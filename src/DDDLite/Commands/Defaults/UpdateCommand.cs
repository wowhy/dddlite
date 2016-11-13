namespace DDDLite.Commands
{
    public class UpdateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}