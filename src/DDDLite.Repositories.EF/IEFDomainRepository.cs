namespace DDDLite.Repositories.EF
{
    using Microsoft.EntityFrameworkCore;

    public interface IEFDomainRepository<TAggregateRoot> : IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        DbContext Context { get; }
    }
}
