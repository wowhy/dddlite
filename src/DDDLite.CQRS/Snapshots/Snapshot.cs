namespace DDDLite.CQRS.Snapshots
{
  using System;

  using DDDLite.Domain;

  public class Snapshot : ISnapshot, ILogicalDelete
  {
    public Guid Id { get; set; }

    public long Version { get; set; }

    public bool Deleted { get; set; }
  }
}