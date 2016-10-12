namespace DDDLite.Commands
{
    using System.Collections.Generic;
    
    using Domain;
    using Validation;

    public interface IDomainCommandHandler<TCommand, TAggregateRoot> : ICommandHandler<TCommand>
        where TCommand : IDomainCommand<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        ICollection<IValidator> Validators { get; }

        void Validate(TCommand command);
    }
}
