namespace DDDLite.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain;
    using Specifications;

    public interface IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        IDomainRepositoryContext Context { get; }

        void Create(TAggregateRoot entity);

        void Update(TAggregateRoot entity);

        void Delete(TAggregateRoot entity);

        bool Exist(Specification<TAggregateRoot> specification);

        TAggregateRoot GetById(Guid id);

        IQueryable<TAggregateRoot> Find();

        IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        Task<TAggregateRoot> GetByIdAsync(Guid id);
    }
}
