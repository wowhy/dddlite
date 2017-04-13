namespace DDDLite.Commands
{
    using System;

    public class AggregateRootCommand<TAggregateRoot> : Command, IAggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected AggregateRootCommand()
        {
        }

        public Guid AggregateRootId { get; set; }

        public TAggregateRoot AggregateRoot { get; set; }

        public long RowVersion { get; set; }

        public virtual bool RowVersionUp => true;

        IAggregateRoot IAggregateRootCommand.AggregateRoot
        {
            get
            {
                return this.AggregateRoot;
            }

            set
            {
                this.AggregateRoot = value as TAggregateRoot;
            }
        }
    }
}
