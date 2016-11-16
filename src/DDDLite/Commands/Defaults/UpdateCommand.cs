namespace DDDLite.Commands
{
    public class UpdateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}