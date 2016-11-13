namespace DDDLite.Commands
{
    using System;

    public class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected DomainCommand()
        { }

        public Guid AggregateRootId { get; set; }

        public TAggregateRoot AggregateRoot { get; set; }

        public long RowVersion { get; set; }
    }
}
