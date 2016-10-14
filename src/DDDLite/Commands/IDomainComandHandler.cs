namespace DDDLite.Commands
{
    using System.Collections.Generic;

    using Domain;
    using Repository;
    using Validation;

    public interface IDomainCommandHandler<TCommand, TAggregateRoot> : ICommandHandler<TCommand>
        where TCommand : IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        IDomainRepositoryContext Context { get; }

        IDomainRepository<TAggregateRoot> Repository { get; }

        ICollection<IValidator> Validators { get; }

        void Validate(TCommand command);
    }
}
