namespace DDDLite.Repositories.EntityFramework
{
  using System;
  using DDDLite.Domain;
  using Microsoft.EntityFrameworkCore;

  public interface IEFRepository<TEntity, TKey> : IRepository<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    DbContext Context { get; }
  }
}
