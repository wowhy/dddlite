namespace DDDLite.Commands
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : IHandler<TCommand>, ICommandHandler
        where TCommand : class, ICommand
    {
        void Validate(TCommand command);
    }
}
