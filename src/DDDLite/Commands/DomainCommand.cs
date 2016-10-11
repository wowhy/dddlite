namespace DDDLite.Commands
{
    using System;

    using Domain;
    using Validation;

    public class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly ValidationCollection validators = new ValidationCollection();

        protected DomainCommand()
        { }

        protected DomainCommand(params IValidator[] validators) : base(validators)
        { }

        public Guid AggregateRootId { get; set; }

        public TAggregateRoot AggregateRoot { get; set; }

        public Guid OperatorId { get; set; }

        public long RowVersion { get; set; }
    }
}
