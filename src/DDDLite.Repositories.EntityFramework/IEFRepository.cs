namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using DDDLite.Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IEFRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        DbContext Context { get; }
    }
}
