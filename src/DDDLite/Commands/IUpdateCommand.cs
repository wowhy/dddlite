namespace DDDLite.Commands
{
    using Domain;

    public interface IUpdateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}