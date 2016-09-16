namespace DDDLite.Domain.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Domain.Core;
    using Specifications;

    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot Get(Guid key);

        Task<TAggregateRoot> GetAsync(Guid key);

        IQueryable<TAggregateRoot> FindAll();

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        void Add(TAggregateRoot entity);

        void Update(TAggregateRoot entity);

        void Remove(TAggregateRoot entity);

        bool Exists(Specification<TAggregateRoot> specification);
    }
}
