namespace DDDLite.Commands
{
    using System;
    using System.Dynamic;

    using Domain;

    public interface IDynamicCommand : ICommand
    {
        DynamicObject Bags { get; }
    }

    public interface IDomainCommand<T> : IDynamicCommand
        where T : class, IAggregateRoot
    {
        T Data { get; set; }

        Guid? OperatorId { get; set; }
    }

    public interface ICreateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
    }

    public interface IUpdateCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {
        Guid AggregateRootId { get; set; }
        
        long RowVersion { get; set; }
    }

    public interface IDeleteCommand<T> : IDomainCommand<T>
        where T : class, IAggregateRoot
    {

        Guid AggregateRootId { get; set; }

        long RowVersion { get; set; }
    }
}
