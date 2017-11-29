namespace DDDLite.CQRS
{
  using System;
  using DDDLite.Domain;

  public class Dto : Entity<Guid>, IDto
  {
    public long Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string LastUpdatedById { get; set; }
  }
}