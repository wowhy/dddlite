namespace DDDLite.Commands
{
    using System.Threading.Tasks;

    using Domain;
    using Validation;

    public class CreateCommandHandler<TAggregateRoot> :
        DomainCommandHandler<CreateCommand<TAggregateRoot>, TAggregateRoot>
        , ICreateCommandHandler<CreateCommand<TAggregateRoot>, TAggregateRoot>
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

        public override Task DoHandleAsync(CreateCommand<TAggregateRoot> command)
        {
            var entity = new TAggregateRoot();

            this.Map(command.AggregateRoot, entity);

            entity.CreatedById = command.OperatorId;
            entity.CreatedOn = command.Timestamp;
            entity.RowVersion = command.RowVersion;

            this.Repository.Create(entity);

            return this.Context.CommitAsync();
        }
    }
}
