namespace DDDLite.Commands
{
    using Domain;

    public interface IDeleteCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}