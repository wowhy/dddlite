namespace DDDLite.Commands
{
    using Domain;
    using Validation;

    public class DeleteCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public DeleteCommand(params IValidator[] validators)
            : base(validators)
        {
        }
    }
}