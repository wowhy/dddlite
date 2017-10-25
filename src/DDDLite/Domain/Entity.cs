namespace DDDLite.Domain
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public TKey Id { get; set; }
  }
}
