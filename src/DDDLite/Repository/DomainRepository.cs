namespace DDDLite.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain;
    using Specifications;

    public abstract class DomainRepository<TAggregateRoot> : IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDomainRepositoryContext context;

        public IDomainRepositoryContext Context => this.context;

        protected DomainRepository(IDomainRepositoryContext context)
        {
            this.context = context;
        }

        public abstract void Create(TAggregateRoot entity);

        public abstract void Update(TAggregateRoot entity);

        public abstract void Delete(TAggregateRoot entity);

        public abstract TAggregateRoot GetById(Guid id);

        public virtual IQueryable<TAggregateRoot> Find()
        {
            return this.Find(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification)
        {
            return this.Find(specification, null);
        }

        public abstract IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        public abstract Task<TAggregateRoot> GetByIdAsync(Guid id);

        public abstract bool Exist(Specification<TAggregateRoot> specification);
    }
}
