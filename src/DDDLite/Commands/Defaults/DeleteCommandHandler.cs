namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Repository;
    using Validation;

    public class DeleteCommandHandler<TAggregateRoot> :
        DomainCommandHandler<DeleteCommand<TAggregateRoot>, TAggregateRoot>
        , IDeleteCommandHandler<DeleteCommand<TAggregateRoot>, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        public DeleteCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        public DeleteCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
            : base(context, validators)
        {
        }

        public override Task DoHandleAsync(DeleteCommand<TAggregateRoot> command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            entity.RowVersion = command.RowVersion;

            this.Repository.Delete(entity);

            return this.Context.CommitAsync();
        }
    }
}
