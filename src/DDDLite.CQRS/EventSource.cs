namespace DDDLite.CQRS
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using DDDLite.Domain;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Exceptions;
  using System.Reflection;

  public abstract class EventSource : IEventSource, ILogicalDelete
  {
    private readonly List<IEvent> undispatchedEvents = new List<IEvent>();

    public Guid Id { get; set; }

    public long Version { get; set; } = -1;

    public bool Deleted { get; set; }

    public EventSource()
    {
    }

    public IEvent[] FlushUndispatchedEvents()
    {
      lock (undispatchedEvents)
      {
        var changes = undispatchedEvents.ToArray();

        for (var i = 0; i < changes.Length; i++)
        {
          var @event = changes[i];
          if (@event.Id == Guid.Empty && Id == Guid.Empty)
          {
            throw new AggregateOrEventMissingIdException(GetType(), @event.GetType());
          }

          if (@event.Id == Guid.Empty)
          {
            @event.Id = Id;
          }

          @event.Version = Version + i + 1;
        }

        Version = Version + changes.Length;
        undispatchedEvents.Clear();
        return changes;
      }
    }

    public IEvent[] GetUndispatchedEvents()
    {
      lock (undispatchedEvents)
      {
        return undispatchedEvents.ToArray();
      }
    }

    public void LoadFromHistory(IEnumerable<IEvent> histories)
    {
      lock (undispatchedEvents)
      {
        var events = histories.ToArray();
        if (events.Length == 0)
        {
          return;
        }

        foreach (var @event in events)
        {
          if (@event.Version != Version + 1)
          {
            throw new EventsOutOfOrderException(@event.Id);
          }

          InvokeEvent(@event);

          Id = @event.Id;
          Version++;
        }
      }
    }

    public void ApplyEvent(IEvent @event)
    {
      lock (undispatchedEvents)
      {
        InvokeEvent(@event);
        undispatchedEvents.Add(@event);
      }
    }

    protected virtual void InvokeEvent(IEvent @event)
    {
      var eventType = @event.GetType();
      var method = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                       .Where(m =>
                       {
                         var flag = m.Name == "Apply";
                         if (flag)
                         {
                           var parameters = m.GetParameters();
                           flag = parameters.Length == 1 && parameters.First().ParameterType == eventType;
                         }
                         return flag;
                       })
                       .FirstOrDefault();

      if (method != null)
      {
        method.Invoke(this, new object[] { @event });
      }
    }
  }
}