namespace DDDLite.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        Task<TAggregateRoot> GetByIdAsync(Guid key);

        Task SaveAsync(TAggregateRoot aggregateRoot);

        Task DeleteAsync(TAggregateRoot aggregateRoot);
    }
}