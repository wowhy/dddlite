namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Repository;
    using Validation;

    public class DeleteCommandHandler<TCommand, TAggregateRoot> :
        DomainCommandHandler<TCommand, TAggregateRoot>
        , IDeleteCommandHandler<TCommand, TAggregateRoot>
        where TCommand : IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public DeleteCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        public DeleteCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
            : base(context, validators)
        {
        }

        public override Task DoHandleAsync(TCommand command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            entity.RowVersion = command.RowVersion;

            this.Repository.Delete(entity);

            return this.Context.CommitAsync();
        }
    }
}
