namespace DDDLite.CQRS.Commands
{
  using System;
  using DDDLite.CQRS.Messages;
  public interface ICommand : IMessage
  {
    Guid AggregateRootId { get; set; }
    long RowVersion { get; set; }
    string OperatorId { get; set; }
  }
}