namespace DDDLite.EntityFramework.QueryStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.QueryStack.Repository;

    public interface IEFQueryRepositoryContext : IQueryRepositoryContext
    {
        DbContext DbContext { get; }
    }
}