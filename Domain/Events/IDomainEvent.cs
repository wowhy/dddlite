namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDomainEvent
    {
        Guid Id { get; set; }

        Guid AggregateRootKey { get; set; }

        string AggregateRootType { get; set; }

        DateTime Timestamp { get; set; }
    }
}
