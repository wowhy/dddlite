namespace DDDLite.CQRS.Commands
{
  using System;

  public class Command : ICommand
  {
    public Guid AggregateRootId { get; set; }

    public long RowVersion { get; set; }

    public string OperatorId { get; set; }
  }
}