namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Validation;

    public class UpdateCommandHandler<TAggregateRoot> :
        DomainCommandHandler<UpdateCommand<TAggregateRoot>, TAggregateRoot>
        , IUpdateCommandHandler<UpdateCommand<TAggregateRoot>, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        public UpdateCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        public UpdateCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
            : base(context, validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }

        public override Task DoHandleAsync(UpdateCommand<TAggregateRoot> command)
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
