namespace DDDLite.Messaging
{
    using System;

    using Commands;
    
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> GetHandler<T>() where T : class, ICommand;

        ICommandHandler GetHandler(Type commandType);
    }
}
