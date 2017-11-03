namespace DDDLite.CQRS.Commands
{
  using DDDLite.CQRS.Messages;
  public interface ICommand : IMessage
  {
    long RowVersion { get;set; }
  }
}