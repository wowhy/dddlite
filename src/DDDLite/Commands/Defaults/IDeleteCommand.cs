namespace DDDLite.Commands
{
    public interface IDeleteCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}