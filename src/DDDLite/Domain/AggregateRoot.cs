namespace DDDLite.Domain
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
  {
    [ConcurrencyCheck]
    public long RowVersion { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string LastUpdatedById { get; set; }
  }
}
