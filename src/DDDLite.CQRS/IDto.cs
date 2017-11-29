namespace DDDLite.CQRS
{
  using System;

  using DDDLite.Domain;

  public interface IDto : IEntity<Guid>, ITrackable
  {
    long Version { get; set; }
  }
}