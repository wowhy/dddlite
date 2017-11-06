namespace DDDLite.CQRS.Commands
{
  using System;
  using DDDLite.CQRS.Messaging;
  public interface ICommand : IMessage
  {
    Guid Id { get; set; }
    long OriginalVersion { get; set; }
    string OperatorId { get; set; }
  }
}