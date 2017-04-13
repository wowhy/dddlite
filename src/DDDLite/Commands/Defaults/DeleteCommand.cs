namespace DDDLite.Commands
{
    public class DeleteCommand<TAggregateRoot> : AggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}