namespace DDDLite.EntityFramework.CommandStack.Repository
{
    using Microsoft.EntityFrameworkCore;

    using DDDLite.CommandStack.Repository;

    public interface IEFDomainRepositoryContext : IDomainRepositoryContext
    {
        DbContext DbContext { get; }
    }
}