namespace DDDLite.Repository.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.Domain;

    public interface IEFQueryRepositoryContext : IQueryRepositoryContext
    {
        DbContext DbContext { get; }
    }
}