namespace DDDLite.CQRS.Messaging.InMemory
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Commands;
  public class InMemoryCommandBus : InMemoryMessageBus, ICommandSender
  {
    public Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
      return base.CommitAsync(command);
    }
  }
}