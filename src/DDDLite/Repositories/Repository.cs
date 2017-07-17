namespace DDDLite.Repositories
{
    using DDDLite.Domain;
    using System;
    using System.Threading.Tasks;
    using DDDLite.Specifications;
    using System.Linq;

    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected Repository()
        {
        }

        public abstract Task AddAsync(TAggregateRoot entity);
        public abstract Task UpdateAsync(TAggregateRoot entity);
        public abstract Task DeleteAsync(TAggregateRoot entity);
        public abstract Task<TAggregateRoot> GetByIdAsync(Guid id);

        public virtual IQueryable<TAggregateRoot> Search()
        {
            return this.Search(Specification<TAggregateRoot>.Any(), SortSpecification<TAggregateRoot>.None);
        }

        public IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter)
        {
            return this.Search(filter, SortSpecification<TAggregateRoot>.None);
        }

        public IQueryable<TAggregateRoot> Search(SortSpecification<TAggregateRoot> sorter)
        {
            return this.Search(Specification<TAggregateRoot>.Any(), sorter);
        }

        public abstract IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter);
    }
}
