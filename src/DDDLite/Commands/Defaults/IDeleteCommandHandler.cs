namespace DDDLite.Commands
{
    using Domain;

    public interface IDeleteCommandHandler<TCommand, TAggregateRoot> : IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
