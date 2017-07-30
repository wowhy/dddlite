namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using DDDLite.Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IEFRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        DbContext Context { get; }
    }
}
