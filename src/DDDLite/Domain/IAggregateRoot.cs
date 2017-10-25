namespace DDDLite.Domain
{
  using System;

  public interface IAggregateRoot<TKey> : IEntity<TKey>, IConcurrencyToken, ITrackable
    where TKey : IEquatable<TKey>
  {
  }
}