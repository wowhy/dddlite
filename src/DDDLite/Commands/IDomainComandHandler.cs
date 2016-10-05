namespace DDDLite.Commands
{
    using Domain;

    public interface IDomainCommandHandler<TCommand, TData> : ICommandHandler<TCommand>
        where TCommand : IDomainCommand<TData>
        where TData : class, IAggregateRoot
    {
    }

    public interface ICreateCommandHandler<TCommand, TData> : IDomainCommandHandler<TCommand, TData>
        where TCommand : ICreateCommand<TData>
        where TData : class, IAggregateRoot
    {
    }

    public interface IUpdateCommandHandler<TCommand, TData> : IDomainCommandHandler<TCommand, TData>
        where TCommand : IUpdateCommand<TData>
        where TData : class, IAggregateRoot
    {
    }

    public interface IDeleteCommandHandler<TCommand, TData> : IDomainCommandHandler<TCommand, TData>
        where TCommand : IDeleteCommand<TData>
        where TData : class, IAggregateRoot
    {
    }
}
