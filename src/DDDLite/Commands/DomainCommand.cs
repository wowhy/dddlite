namespace DDDLite.Commands
{
    using System;

    using Domain;

    public class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected DomainCommand()
        { }

        public Guid AggregateRootId { get; set; }

        public TAggregateRoot AggregateRoot { get; set; }

        public Guid OperatorId { get; set; }

        public long RowVersion { get; set; }
    }
}
