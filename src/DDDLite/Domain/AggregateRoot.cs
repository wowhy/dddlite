namespace DDDLite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Events;

    public abstract class AggregateRoot : Entity, Domain.IAggregateRoot
    {        
        private readonly Queue<IEvent> uncommitedEvents = new Queue<IEvent>();
   
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RowVersion { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<IEvent> UncommittedEvents => this.uncommitedEvents;

        public void RaiseEvent<TEvent>(TEvent e) where TEvent : IEvent
        {
            this.ApplyEvent(e);
        }

        protected void ApplyEvent<TEvent>(TEvent e) where TEvent : IEvent
        {
            this.uncommitedEvents.Enqueue(e);
        }
    }
}
