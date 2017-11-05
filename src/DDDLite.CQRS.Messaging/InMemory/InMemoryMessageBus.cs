namespace DDDLite.CQRS.Messaging.InMemory
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Immutable;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.CQRS.Messages;

  public abstract class InMemoryMessageBus
  {
    private readonly Dictionary<Type, List<Func<IMessage, Task>>> _routes = new Dictionary<Type, List<Func<IMessage, Task>>>();

    protected ImmutableDictionary<Type, List<Func<IMessage, Task>>> Routes => _routes.ToImmutableDictionary();

    protected void AddHandler<T>(Func<IMessage, Task> handler) where T : class, IMessage
    {
      List<Func<IMessage, Task>> handlers;

      if (!_routes.TryGetValue(typeof(T), out handlers))
      {
        handlers = new List<Func<IMessage, Task>>();
        _routes.Add(typeof(T), handlers);
      }

      handlers.Add((x => handler((T)x)));
    }

    protected async Task CommitAsync(IMessage message)
    {
      try
      {
        List<Func<IMessage, Task>> handlers;

        if (_routes.TryGetValue(message.GetType(), out handlers))
        {
          if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
          await handlers[0](message);
        }
        else
        {
          throw new InvalidOperationException("no handler registered");
        }
      }
      catch
      {
        // nothing
      }
    }

    protected async Task DispatchAsync(IMessage @event)
    {
      List<Func<IMessage, Task>> handlers;

      if (!_routes.TryGetValue(@event.GetType(), out handlers))
        return;


      foreach (var handler in handlers)
      {
        try
        {
          await handler(@event);
        }
        catch
        {
          // nothing
        }
      }
    }
  }
}