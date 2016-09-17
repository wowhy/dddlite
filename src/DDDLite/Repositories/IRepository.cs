namespace DDDLite.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Specifications;

    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot Get(Guid key);

        Task<TAggregateRoot> GetAsync(Guid key);

        IQueryable<TAggregateRoot> FindAll();

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        void Insert(TAggregateRoot entity);

        void Update(TAggregateRoot entity);

        void Delete(TAggregateRoot entity);

        bool Exists(Specification<TAggregateRoot> specification);
    }
}
