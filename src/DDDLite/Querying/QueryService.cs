namespace DDDLite.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Repository;

    public abstract class QueryService<TAggregateRoot> : IQueryService<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public static readonly ICollection<Sorter> EmptySorters = new List<Sorter>();
        public static readonly ICollection<Filter> EmptyFilters = new List<Filter>();

        private readonly IQueryRepositoryContext context;
        private readonly IQueryRepository<TAggregateRoot> repository;

        protected QueryService(IQueryRepositoryContext context)
        {
            this.context = context;
            this.repository = context.GetRepository<TAggregateRoot>();
        }

        public IQueryRepositoryContext Context => this.context;

        public IQueryRepository<TAggregateRoot> Repository => this.repository;

        public virtual TDTO GetById<TDTO>(Guid id) where TDTO : class, new()
        {
            return this.Repository.GetById<TDTO>(id);
        }

        public virtual IQueryable<TDTO> Find<TDTO>() where TDTO : class, new()
        {
            return this.Find<TDTO>(EmptyFilters, EmptySorters);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(ICollection<Sorter> sorters) where TDTO : class, new()
        {
            return this.Find<TDTO>(EmptyFilters, sorters);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(ICollection<Filter> filters) where TDTO : class, new()
        {
            return this.Find<TDTO>(filters, EmptySorters);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(ICollection<Filter> filters, ICollection<Sorter> sorters) where TDTO : class, new()
        {
            return this.Repository.Find<TDTO>(filters.ToSpecification<TAggregateRoot>(), sorters.ToSpecification<TAggregateRoot>());
        }

        public PagedResult<TDTO> Page<TDTO>(int page, int limit) where TDTO : class, new()
        {
            return this.Page<TDTO>(page, limit, EmptyFilters, EmptySorters);
        }

        public PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Filter> filters) where TDTO : class, new()
        {
            return this.Page<TDTO>(page, limit, filters, EmptySorters);
        }

        public virtual PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Sorter> sorters) where TDTO : class, new()
        {
            return this.Page<TDTO>(page, limit, EmptyFilters, sorters);
        }

        public PagedResult<TDTO> Page<TDTO>(int page, int limit, ICollection<Filter> filters, ICollection<Sorter> sorters) where TDTO : class, new()
        {
            return this.Repository.Find<TDTO>(filters.ToSpecification<TAggregateRoot>(), sorters.ToSpecification<TAggregateRoot>()).AsPagedResult(page, limit);
        }
    }
}
