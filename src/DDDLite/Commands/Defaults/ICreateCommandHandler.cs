namespace DDDLite.Commands
{
    using Domain;

    public interface ICreateCommandHandler<TCommand, TAggregateRoot> : IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, new()
    {
    }
}
