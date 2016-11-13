namespace DDDLite.Events
{
    using System;

    public abstract class DomainEvent<TAggregateRoot> : Message, IDomainEvent<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected DomainEvent()
        {
            this.EventName = this.GetTypeName();
        }


        public long Version { get; set; }

        public Guid AggregateRootId { get; set; }

        public object AggregateRoot { get; set; }

        public string AggregateRootType { get; set; }

        public string EventName { get; set; }
    }
}
