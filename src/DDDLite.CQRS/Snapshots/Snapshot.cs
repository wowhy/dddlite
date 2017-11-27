namespace DDDLite.CQRS.Snapshots
{
  using System;

  using DDDLite.Domain;

  public class Snapshot : AggregateRoot<Guid>, ISnapshot, ILogicalDelete
  {
    public bool Deleted { get; set; }
  }
}