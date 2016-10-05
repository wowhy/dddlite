namespace DDDLite.Domain
{
    using System;

    public interface IAggregateRoot : IEntity
    {
        long RowVersion { get; set; }

        Guid? CreatedById { get; set; }

        DateTime CreatedOn { get; set; }

        Guid? ModifiedById { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
