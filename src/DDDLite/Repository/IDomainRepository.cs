namespace DDDLite.Repository
{
    using System;
    using System.Linq;

    using Specifications;

    public interface IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        void Save(TAggregateRoot entity);

        bool Exist(Specification<TAggregateRoot> specification);

        TAggregateRoot GetById(Guid id);

        IQueryable<TAggregateRoot> Find();

        IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification);

        IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);
    }
}
