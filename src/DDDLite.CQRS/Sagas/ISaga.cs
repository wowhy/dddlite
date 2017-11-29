namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Collections.Generic;

  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Events;
  using DDDLite.Domain;

  public interface ISaga
  {
    Guid Id { get; set; }

    bool Completed { get; set; }

    void MarkComplete();

    ICommand[] GetUncommittedCommands();

    ICommand[] FlushUncommittedCommands();
  }
}