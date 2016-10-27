namespace DDDLite.Commands
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {
        void Validate(TCommand command);
    }
}
