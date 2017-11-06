namespace DDDLite.CQRS.Messaging.InMemory
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Messaging;

  public class InMemoryCommandBus : InMemoryMessageBus, ICommandSender
  {
    public Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
      return base.CommitAsync(command);
    }
  }
}