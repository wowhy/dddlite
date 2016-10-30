namespace DDDLite.Repository
{
    using System;
    using System.Linq;


    using Domain;
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

        public abstract IQueryable<TAggregateRoot> QueryModel { get; }

        public abstract TDTO GetById<TDTO>(Guid id) where TDTO : class, new();

        public virtual IQueryable<TDTO> Find<TDTO>() where TDTO : class, new()
        {
            return this.Find<TDTO>(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification) where TDTO : class, new()
        {
            return this.Find<TDTO>(specification, null);
        }

        public abstract IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification) where TDTO : class, new();
    }
}
