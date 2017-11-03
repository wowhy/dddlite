namespace DDDLite.CQRS.Events
{
  using DDDLite.CQRS.Messages;
  public interface IEventHandler<TEvent> : IHandler<TEvent>
    where TEvent : IEvent
  {
  }
}