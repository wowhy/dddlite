namespace Domain.Repository
{
    using System;
    using Domain.Core;

    public interface IRepository<TEntity> where TEntity : Entity
    {
    }
}
