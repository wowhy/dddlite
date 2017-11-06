namespace DDDLite.CQRS
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using DDDLite.Domain;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Exceptions;
  using System.Reflection;

  public abstract class EventSource : AggregateRoot<Guid>, IEventSource
  {
    private readonly List<IEvent> uncommitedChanges = new List<IEvent>();

    public EventSource()
    {
      this.Version = -1;
    }

    public IEvent[] FlushUncommitedChanges()
    {
      lock (uncommitedChanges)
      {
        var changes = uncommitedChanges.ToArray();
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
          @event.Timestamp = DateTime.Now;
        }

        if (changes.Length > 0)
        {
          if (Version == -1)
          {
            CreatedAt = changes[0].Timestamp;
            CreatedById = changes[0].OperatorId;
          }

          LastUpdatedAt = changes[changes.Length - 1].Timestamp;
          LastUpdatedById = changes[changes.Length - 1].OperatorId;
        }

        Version = Version + changes.Length;
        uncommitedChanges.Clear();
        return changes;
      }
    }

    public IEvent[] GetUncommittedChanges()
    {
      lock (uncommitedChanges)
      {
        return uncommitedChanges.ToArray();
      }
    }

    public void LoadFromHistory(IEnumerable<IEvent> histories)
    {
      lock (uncommitedChanges)
      {
        var events = histories.ToArray();
        if (events.Length == 0)
        {
          return;
        }

        if (Version == -1)
        {
          CreatedAt = events[0].Timestamp;
          CreatedById = events[0].OperatorId;
        }

        foreach (var @event in events)
        {
          if (@event.Version != Version + 1)
          {
            throw new EventsOutOfOrderException(@event.Id);
          }

          ApplyEvent(@event);

          Id = @event.Id;
          Version++;
          LastUpdatedAt = @event.Timestamp;
          LastUpdatedById = @event.OperatorId;
        }
      }
    }

    public void ApplyChange(IEvent @event)
    {
      lock (uncommitedChanges)
      {
        ApplyEvent(@event);
        uncommitedChanges.Add(@event);
      }
    }

    protected virtual void ApplyEvent(IEvent @event)
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