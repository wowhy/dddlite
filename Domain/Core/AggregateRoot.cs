namespace Domain.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AggregateRoot : Entity, IAggregateRoot
    {
        [ConcurrencyCheck]
        public long RowVersion { get; set; }
    }
}
