namespace DDDLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Events;

    public abstract class AggregateRoot : Entity, IAggregateRoot, IConcurrencyVersion
    {        
        private readonly Queue<IDomainEvent> uncommitedEvents = new Queue<IDomainEvent>();
   
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RowVersion { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<IDomainEvent> UncommittedEvents => this.uncommitedEvents;

        public void RaiseEvent<TEvent>(TEvent e) where TEvent : class, IDomainEvent
        {
            this.ApplyEvent(e);
        }

        protected void ApplyEvent<TEvent>(TEvent e) where TEvent : class, IDomainEvent
        {
            e.AggregateRootId = this.Id;
            e.AggregateRoot = this;
            e.AggregateRootType = this.GetType().FullName;
            this.uncommitedEvents.Enqueue(e);
        }
    }
}
