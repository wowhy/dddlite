namespace DDDLite.Commands
{
    using Domain;

    public interface IUpdateCommandHandler<TCommand, TAggregateRoot> : IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
