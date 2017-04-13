namespace DDDLite
{
    using System;
    using Commands;
    
    public interface IAggregateRoot : IEntity, IConcurrencyVersion
    {
        Guid? CreatedById { get; set; }

        DateTime CreatedOn { get; set; }

        Guid? ModifiedById { get; set; }

        DateTime? ModifiedOn { get; set; }

        void HandleCommand(ICommand command);
    }
}