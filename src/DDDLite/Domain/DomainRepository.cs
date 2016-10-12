namespace DDDLite.Domain
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

        public virtual IQueryable<TAggregateRoot> FindAll()
        {
            return this.FindAll(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification)
        {
            return this.FindAll(specification, null);
        }

        public abstract IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        public abstract Task<TAggregateRoot> GetByIdAsync(Guid id);

        public abstract bool Exist(Specification<TAggregateRoot> specification);
    }
}
