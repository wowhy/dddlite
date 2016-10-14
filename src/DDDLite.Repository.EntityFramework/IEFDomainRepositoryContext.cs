namespace DDDLite.Repository.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.Domain;

    public interface IEFDomainRepositoryContext : IDomainRepositoryContext
    {
        DbContext DbContext { get; }
    }
}