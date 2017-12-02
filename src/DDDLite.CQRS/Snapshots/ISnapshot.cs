namespace DDDLite.CQRS.Snapshots
{
  using System;

  using DDDLite.Domain;

  public interface ISnapshot : ILogicalDelete
  {
    Guid Id { get; set; }

    long Version { get; set; }
  }
}