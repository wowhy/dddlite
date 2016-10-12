namespace DDDLite.QueryStack.Application
{
    using System;
    using System.Linq;

    using Domain;
    using Specifications;

    public abstract class QueryService<TAggregateRoot> : IQueryService<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IQueryRepositoryContext context;
        private readonly IQueryRepository<TAggregateRoot> repository;

        protected QueryService(IQueryRepositoryContext context)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();
        }

        public IQueryRepositoryContext Context => this.context;

        public IQueryRepository<TAggregateRoot> Repository => this.repository;

        public virtual TAggregateRoot GetById(Guid id)
        {
            return this.repository.GetById(id);
        }

        public virtual IQueryable<TAggregateRoot> FindAll()
        {
            return this.repository.FindAll();
        }

        public virtual IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification)
        {
            return this.repository.FindAll(specification);
        }

        public virtual IQueryable<TAggregateRoot> FindAll(
            Specification<TAggregateRoot> specification,
            SortSpecification<TAggregateRoot> sortSpecification
        )
        {
            return this.repository.FindAll(specification, sortSpecification);
        }

        public virtual PagedResult<TAggregateRoot> Page(int page, int limit)
        {
            return this.repository.FindAll().AsPagedResult(page, limit);
        }

        public virtual PagedResult<TAggregateRoot> Page(
            int page,
            int limit,
            Specification<TAggregateRoot> specification)
        {
            return this.repository.FindAll(specification).AsPagedResult(page, limit);
        }

        public virtual PagedResult<TAggregateRoot> Page(
            int page,
            int limit,
            Specification<TAggregateRoot> specification,
            SortSpecification<TAggregateRoot> sortSpecification
        )
        {
            return this.repository.FindAll(specification, sortSpecification).AsPagedResult(page, limit);
        }
    }
}
