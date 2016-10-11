namespace DDDLite.Commands
{
    using System;

    using Domain;


    public interface IDomainCommand<TAggregateRoot> : ICommand
        where TAggregateRoot : class, IAggregateRoot
    {
        Guid AggregateRootId { get; set; }

        TAggregateRoot AggregateRoot { get; set; }

        Guid OperatorId { get; set; }

        long RowVersion { get; set; }
    }
}
