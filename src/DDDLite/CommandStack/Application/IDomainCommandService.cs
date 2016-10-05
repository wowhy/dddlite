namespace DDDLite.CommandStack.Application
{
    using Domain;
    using Commands;

    public interface IDomainCommandService<TAggregateRoot> :
        ICreateCommandHandler<ICreateCommand<TAggregateRoot>, TAggregateRoot>,
        IUpdateCommandHandler<IUpdateCommand<TAggregateRoot>, TAggregateRoot>,
        IDeleteCommandHandler<IDeleteCommand<TAggregateRoot>, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
