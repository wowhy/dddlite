namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DomainEvent : IDomainEvent<Guid>
    {
        protected DomainEvent() { }

        protected DomainEvent(Guid entityKey)
        {
            this.Id = Guid.NewGuid();
            this.EntityKey = entityKey;
            this.Timestamp = DateTime.UtcNow;
            this.EventName = this.GetType().FullName;
        }

        public Guid Id { get; set; }

        public Guid EntityKey { get; set; }

        public string EntityType { get; set; }

        public string EventName { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
