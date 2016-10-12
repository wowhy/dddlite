namespace DDDLite.Domain
{
    using System;
    using System.Linq;

    using Specifications;

    public abstract class QueryRepository<TAggregateRoot> : IQueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IQueryRepositoryContext context;

        public IQueryRepositoryContext Context => this.context;

        protected QueryRepository(IQueryRepositoryContext context)
        {
            this.context = context;
        }

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

        public abstract bool Exist(Specification<TAggregateRoot> specification);
    }
}
