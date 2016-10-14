namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Repository;
    using Validation;

    public class CreateCommandHandler<TCommand, TAggregateRoot> :
        DomainCommandHandler<TCommand, TAggregateRoot>
        , ICreateCommandHandler<TCommand, TAggregateRoot>
        where TCommand : ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, new()
    {
        public CreateCommandHandler(IDomainRepositoryContext context) : this(context, null)
        {
        }

        public CreateCommandHandler(IDomainRepositoryContext context, params IValidator[] validators)
            : base(context, validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }

        public override Task DoHandleAsync(TCommand command)
        {
            var entity = new TAggregateRoot();

            this.Map(command.AggregateRoot, entity);

            entity.Id = command.AggregateRootId;
            entity.CreatedById = command.OperatorId;
            entity.CreatedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;

            this.Repository.Create(entity);

            return this.Context.CommitAsync();
        }
    }
}
