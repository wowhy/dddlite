namespace Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDomainEvent<TEntityKey>
        where TEntityKey : IEquatable<TEntityKey>
    {
        Guid Id { get; set; }

        TEntityKey EntityKey { get; set; }

        string EntityType { get; set; }

        DateTime Timestamp { get; set; }
    }
}
