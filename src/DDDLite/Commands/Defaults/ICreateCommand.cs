namespace DDDLite.Commands
{
    public interface ICreateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}