namespace DDDLite.CQRS.Messaging.InMemory
{
  using System.Threading.Tasks;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Events;

  public class InMemoryEventBus : InMemoryMessageBus, IEventPublisher
  {
    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
      return base.DispatchAsync(@event);
    }
  }
}