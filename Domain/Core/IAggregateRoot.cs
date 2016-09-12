namespace Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Events;

    public interface IAggregateRoot : IEntity
    {
        long RowVersion { get; set; }

        Guid? CreatedById { get; set; }

        DateTime CreatedOn { get; set; }

        Guid? ModifiedById { get; set; }

        DateTime? ModifiedOn { get; set; }

        IEnumerable<DomainEvent> UncommittedEvents { get; }

        void Replay(IEnumerable<DomainEvent> events);
    }
}
