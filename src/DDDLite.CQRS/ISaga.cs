namespace DDDLite.CQRS
{
  using System;
  using System.Collections.Generic;

  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Events;
  using DDDLite.Domain;

  public interface ISaga
  {
    Guid Id { get; }
    int Version { get; }

    void Transition(IEvent message);

    IEnumerable<IEvent> GetUncommittedChanges();
    IEnumerable<IEvent> FlushUncommitedChanges();

    IEnumerable<ICommand> GetUnpublishedCommands();
    IEnumerable<ICommand> FlushUnpublishedCommands();
  }
}