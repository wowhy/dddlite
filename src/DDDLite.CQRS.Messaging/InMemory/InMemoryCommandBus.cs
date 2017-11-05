namespace DDDLite.CQRS.Messaging.InMemory
{
  using System;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Messages;

  public class InMemoryCommandBus : InMemoryMessageBus, ICommandSender
  {

    public void RegisterHandler<T>(Func<T, Task> handler) where T : class, ICommand
    {
      base.AddHandler<T>((message) => handler((T)message));
    }
    public Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
      return base.CommitAsync(command);
    }
  }
}