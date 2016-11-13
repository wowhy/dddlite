namespace DDDLite.Commands
{
    public interface IUpdateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}