namespace DDDLite.Commands
{
    using DDDLite.Common;

    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {
        void Validate(TCommand command);
    }
}
