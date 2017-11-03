namespace DDDLite.CQRS.Events
{
  using System.Threading.Tasks;

  public interface IEventPublisher
  {
    Task PublishAsync<TEvent>(TEvent @event)
      where TEvent : IEvent;
  }
}