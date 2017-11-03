namespace DDDLite.CQRS.Snapshots
{
  using System;

  using DDDLite.Domain;

  public class Snapshot : AggregateRoot<Guid>, ISnapshot
  {
  }
}