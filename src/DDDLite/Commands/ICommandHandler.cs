namespace DDDLite.Commands
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : class, ICommand
    {
        void Validate(TCommand command);
    }
}
