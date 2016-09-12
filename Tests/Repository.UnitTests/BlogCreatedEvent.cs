using System;
using Domain.Events;

namespace Repository.UnitTests
{
    public class BlogCreatedEvent : DomainEvent
    {
        public BlogCreatedEvent() : base()
        {
        }

        public BlogCreatedEvent(Guid aggregateRootKey) : base(aggregateRootKey)
        {
        }
    }
}