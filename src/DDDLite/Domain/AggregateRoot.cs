namespace DDDLite.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        [ConcurrencyCheck]
        public long RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Guid? LastUpdatedById { get; set; }
    }
}
