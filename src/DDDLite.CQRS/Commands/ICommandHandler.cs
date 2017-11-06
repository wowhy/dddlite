namespace DDDLite.CQRS.Commands
{
  using DDDLite.CQRS.Messaging;

  public interface ICommandHandler<TCommand> : IHandler<TCommand>
    where TCommand : ICommand
  {
  }
}