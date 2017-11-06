namespace DDDLite.CQRS.Events
{
  using DDDLite.CQRS.Messaging;
  public interface IEventHandler<TEvent> : IHandler<TEvent>
    where TEvent : IEvent
  {
  }
}