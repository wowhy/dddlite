namespace DDDLite.Commands
{
    public class CreateCommand<TAggregateRoot> : AggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}