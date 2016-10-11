namespace DDDLite.Commands
{
    using Domain;

    public interface ICreateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }
}