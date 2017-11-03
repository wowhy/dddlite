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
    }

    public IEvent[] FlushUncommitedChanges()
    {
      lock (uncommitedChanges)
      {
        var changes = uncommitedChanges.ToArray();
        for (var i = 0; i < changes.Length; i++)
        {
          var @event = changes[i];
          if (@event.AggregateRootId == Guid.Empty && Id == Guid.Empty)
          {
            throw new AggregateOrEventMissingIdException(GetType(), @event.GetType());
          }

          if (@event.AggregateRootId == Guid.Empty)
          {
            @event.AggregateRootId = Id;
          }

          @event.RowVersion = RowVersion + i + 1;
          @event.Timestamp = DateTime.Now;
        }

        RowVersion = RowVersion + changes.Length;
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
        foreach (var @event in histories.ToArray())
        {
          if (@event.RowVersion != RowVersion + 1)
          {
            throw new EventsOutOfOrderException(@event.AggregateRootId);
          }
          ApplyEvent(@event);
          Id = @event.AggregateRootId;
          RowVersion++;
        }
      }
    }

    protected void ApplyChange(IEvent @event)
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