namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent() { }

        protected DomainEvent(Guid AggregateRootKey)
        {
            this.Id = SequentialGuid.Create(SequentialGuidType.SequentialAsString);
            this.AggregateRootKey = AggregateRootKey;
            this.Timestamp = DateTime.UtcNow;
            this.EventName = this.GetType().FullName;
        }

        public Guid Id { get; set; }

        public Guid AggregateRootKey { get; set; }

        public string AggregateRootType { get; set; }

        public string EventName { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
