namespace DDDLite.Commands
{
    using System;

    public interface IAggregateRootCommand : ICommand
    {
        Guid AggregateRootId { get; set; }

        IAggregateRoot AggregateRoot { get; set; }

        long RowVersion { get; set; }

        bool RowVersionUp { get; }
    }

    public interface IAggregateRootCommand<TAggregateRoot> : IAggregateRootCommand
        where TAggregateRoot : class, IAggregateRoot
    {
        new TAggregateRoot AggregateRoot { get; set; }
    }
}
