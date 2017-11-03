namespace DDDLite.CQRS.Commands
{
  using DDDLite.CQRS.Messages;

  public interface ICommandHandler<TCommand> : IHandler<TCommand>
    where TCommand : ICommand
  {
  }
}