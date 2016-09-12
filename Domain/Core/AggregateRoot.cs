namespace Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using Events;

    public class AggregateRoot : Entity, IAggregateRoot, IPurgeable
    {
        private readonly Queue<DomainEvent> uncommittedEvents = new Queue<DomainEvent>();

        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [ConcurrencyCheck]
        public long RowVersion { get; set; }

        [NotMapped]
        public IEnumerable<DomainEvent> UncommittedEvents => this.uncommittedEvents;

        public AggregateRoot()
        {
        }

        public void Replay(IEnumerable<DomainEvent> events)
        {
            ((IPurgeable)this).Purge();
            foreach (var evnt in events)
            {
                this.ApplyEvent(evnt);
            }
        }

        protected void ApplyEvent<TEvent>(TEvent evnt) where TEvent : DomainEvent
        {
            var eventHandlerMethods = from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                      let parameters = m.GetParameters()
                                      where m.IsDefined(typeof(InlineEventHandlerAttribute))
                                        && m.ReturnType == typeof(void)
                                        && parameters.Length == 1
                                        && parameters[0].ParameterType == evnt.GetType()
                                      select m;

            evnt.AggregateRootType = this.GetType().FullName;

            foreach (var eventHandlerMethod in eventHandlerMethods)
            {
                eventHandlerMethod.Invoke(this, new object[] { evnt });
            }

            this.uncommittedEvents.Enqueue(evnt);
        }

        void IPurgeable.Purge()
        {
            if (this.uncommittedEvents.Count > 0)
            {
                this.uncommittedEvents.Clear();
            }
        }
    }
}
