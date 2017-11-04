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

    protected void RegisterHandler<T>(Func<IMessage, Task> handler) where T : class, IMessage
    {
      List<Func<IMessage, Task>> handlers;

      if (!_routes.TryGetValue(typeof(T), out handlers))
      {
        handlers = new List<Func<IMessage, Task>>();
        _routes.Add(typeof(T), handlers);
      }

      handlers.Add((x => handler((T)x)));
    }

    protected Task CommitAsync<T>(T command) where T : class, IMessage
    {
      List<Func<IMessage, Task>> handlers;

      if (_routes.TryGetValue(typeof(T), out handlers))
      {
        if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
        return handlers[0](command);
      }
      else
      {
        throw new InvalidOperationException("no handler registered");
      }
    }

    protected Task DispatchAsync<T>(T @event) where T : class, IMessage
    {
      List<Func<IMessage, Task>> handlers;

      if (!_routes.TryGetValue(@event.GetType(), out handlers))
        return Task.CompletedTask;

      return Task.WhenAll(handlers.Select(handler => handler(@event)));
    }
  }
}