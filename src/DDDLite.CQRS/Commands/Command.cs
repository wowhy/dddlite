namespace DDDLite.CQRS.Commands
{
  using System;

  public class Command : ICommand
  {
    public Guid Id { get; set; }

    public long OriginalVersion { get; set; }

    public string OperatorId { get; set; }
  }
}