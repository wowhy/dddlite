namespace DDDLite.Repositories
{
    using DDDLite.Domain;
    using System;
    using System.Threading.Tasks;
    using DDDLite.Specifications;
    using System.Linq;
    using DDDLite.Querying;

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

        public virtual IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter)
        {
            return this.Search(filter, SortSpecification<TAggregateRoot>.None);
        }

        public virtual IQueryable<TAggregateRoot> Search(SortSpecification<TAggregateRoot> sorter)
        {
            return this.Search(Specification<TAggregateRoot>.Any(), sorter);
        }

        public virtual PagedResult<TAggregateRoot> PagedSearch(int page, int limit, SortSpecification<TAggregateRoot> sorter)
        {
            return this.PagedSearch(page, limit, Specification<TAggregateRoot>.Any(), sorter);
        }

		public virtual PagedResult<TAggregateRoot> PagedSearch(int page, int limit, Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter)
		{
            if (sorter == null)
            {
                throw new ArgumentNullException(nameof(sorter));
            }

			var query = this.Search(filter, sorter);
			var count = query.Count();
			var data = query.Skip((page - 1) * limit).Take(limit).ToList();
			return new PagedResult<TAggregateRoot>(count, data);
		}

        public abstract IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter);
    }
}
