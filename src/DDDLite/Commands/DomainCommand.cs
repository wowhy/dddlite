namespace DDDLite.Commands
{
    using System;
    using System.Dynamic;

    using Domain;
    using Validation;

    public class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public class DynamicCommandObject : DynamicObject { }

        private readonly ValidationCollection validators = new ValidationCollection();

        protected DomainCommand()
        { }

        protected DomainCommand(params IValidator[] validators) : base(validators)
        { }

        public DynamicObject Bags { get; protected set; } = new DynamicCommandObject();

        public TAggregateRoot Data { get; set; }

        public Guid? OperatorId { get; set; }
    }

    public class CreateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, ICreateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public CreateCommand(params IValidator[] validators)
            : base(validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }
    }

    public class UpdateCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IUpdateCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public UpdateCommand(params IValidator[] validators)
            : base(validators)
        {
            this.Validators.Add(new EntityValidator<TAggregateRoot>());
        }

        public Guid AggregateRootId { get; set; }

        public long RowVersion { get; set; }
    }

    public class DeleteCommand<TAggregateRoot> : DomainCommand<TAggregateRoot>, IDeleteCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public DeleteCommand(params IValidator[] validators)
            : base(validators)
        {
        }

        public Guid AggregateRootId { get; set; }

        public long RowVersion { get; set; }
    }
}
