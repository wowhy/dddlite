namespace DDDLite.CQRS.Messaging
{
  using System;
  using System.Threading.Tasks;

  public interface IHandlerRegister
  {
    void RegisterHandler<TMessage>(Func<TMessage, Task> handler) where TMessage : IMessage;
  }
}