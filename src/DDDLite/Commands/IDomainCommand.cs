namespace DDDLite.Commands
{
    using System;

    public interface IDomainCommand<TAggregateRoot> : ICommand
        where TAggregateRoot : class, IAggregateRoot
    {
        Guid AggregateRootId { get; set; }

        TAggregateRoot AggregateRoot { get; set; }

        long RowVersion { get; set; }
    }
}
