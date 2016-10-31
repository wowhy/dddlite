namespace DDDLite.Repository.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    using System.Linq;

    public interface IEFQueryRepositoryContext : IQueryRepositoryContext
    {
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters)
            where TEntity : class;
    }
}