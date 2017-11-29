namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Collections.Generic;
  using DDDLite.CQRS.Commands;

  public class Saga : ISaga
  {
    private readonly List<ICommand> uncommittedCommands = new List<ICommand>();

    public Guid Id { get; set; }

    public bool Completed { get; set; } = false;

    public void MarkComplete()
    {
      this.Completed = true;
    }

    public ICommand[] FlushUncommittedCommands()
    {
      lock (uncommittedCommands)
      {
        var changes = uncommittedCommands.ToArray();
        uncommittedCommands.Clear();
        return changes;
      }
    }

    public ICommand[] GetUncommittedCommands()
    {
      lock (uncommittedCommands)
      {
        return uncommittedCommands.ToArray();
      }
    }

    protected void AddCommand<TCommand>(TCommand command)
      where TCommand : class, ICommand
    {
      lock (uncommittedCommands)
      {
        this.uncommittedCommands.Add(command);
      }
    }
  }
}