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

        public abstract TDTO GetById<TDTO>(Guid id) where TDTO : class, new();

        public virtual IQueryable<TDTO> FindAll<TDTO>() where TDTO : class, new()
        {
            return this.FindAll<TDTO>(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TDTO> FindAll<TDTO>(Specification<TAggregateRoot> specification) where TDTO : class, new()
        {
            return this.FindAll<TDTO>(specification, null);
        }

        public abstract IQueryable<TDTO> FindAll<TDTO>(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification) where TDTO : class, new();
    }
}
