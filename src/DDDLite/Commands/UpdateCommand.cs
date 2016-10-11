namespace DDDLite.Commands
{
    using Domain;
    using Validation;

    public class UpdateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public UpdateCommand(params IValidator[] validators)
            : base(validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }
    }
}