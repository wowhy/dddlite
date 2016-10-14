namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Repository;
    using Validation;

    public class UpdateCommandHandler<TCommand, TAggregateRoot> :
        DomainCommandHandler<TCommand, TAggregateRoot>
        , IUpdateCommandHandler<TCommand, TAggregateRoot>
        where TCommand : IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public UpdateCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        public UpdateCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
            : base(context, validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }

        public override Task DoHandleAsync(TCommand command)
        {
            var entity = this.Repository.GetById(command.AggregateRootId);

            this.Map(command.AggregateRoot, entity);

            entity.ModifiedById = command.OperatorId;
            entity.ModifiedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;

            this.Repository.Update(entity);

            return this.Context.CommitAsync();
        }
    }
}
