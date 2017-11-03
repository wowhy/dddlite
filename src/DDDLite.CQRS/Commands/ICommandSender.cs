namespace DDDLite.CQRS.Commands
{
  using System.Threading.Tasks;

  public interface ICommandSender
  {
    Task SendAsync<TCommand>(TCommand command)
      where TCommand : ICommand;
  }
}