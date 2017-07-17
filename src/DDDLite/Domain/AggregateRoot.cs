namespace DDDLite.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        [ConcurrencyCheck]
        public long RowVersion { get; set; }
    }
}
