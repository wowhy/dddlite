namespace DDDLite.Commands
{
    using Domain;
    using Validation;

    public class CreateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public CreateCommand(params IValidator[] validators)
            : base(validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }
    }
}