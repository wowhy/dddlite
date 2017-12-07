namespace DDDLite.CQRS.Messaging.InMemory
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Immutable;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.CQRS.Messaging;
  using Microsoft.Extensions.Logging;

  public abstract class InMemoryMessageBus : IHandlerRegister
  {
    private readonly Dictionary<Type, List<Func<IMessage, Task>>> _routes = new Dictionary<Type, List<Func<IMessage, Task>>>();

    protected ImmutableDictionary<Type, List<Func<IMessage, Task>>> Routes => _routes.ToImmutableDictionary();

    public ILogger Logger { get; protected set; }

    public void RegisterHandler<TMessage>(Func<TMessage, Task> handler) where TMessage : IMessage
    {
      List<Func<IMessage, Task>> handlers;

      if (!_routes.TryGetValue(typeof(TMessage), out handlers))
      {
        handlers = new List<Func<IMessage, Task>>();
        _routes.Add(typeof(TMessage), handlers);
      }

      handlers.Add((x => handler((TMessage)x)));
    }

    protected async Task CommitAsync(IMessage message)
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
        catch(Exception ex)
        {
          this.Logger?.LogError(0, ex, ex.Message);
        }
      }
    }
  }
}