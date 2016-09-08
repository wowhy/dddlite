namespace Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Core;

    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity Get(Guid key);

        IQueryable<TEntity> GetAll();

        bool Exists();
    }
}
