namespace DDDLite.Commands
{
    public class UpdateCommand<TAggregateRoot> : AggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public override bool RowVersionUp => false;
    }
}