namespace DDDLite.CQRS.Messages
{
  using System.Threading.Tasks;

  public interface IHandler<in TMessage> where TMessage : IMessage
  {
    Task HandleAsync(TMessage message);
  }
}