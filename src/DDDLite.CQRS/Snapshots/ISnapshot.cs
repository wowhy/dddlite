namespace DDDLite.CQRS.Snapshots
{
  using System;
  
  using DDDLite.Domain;

  public interface ISnapshot : IAggregateRoot<Guid>
  {
  }
}