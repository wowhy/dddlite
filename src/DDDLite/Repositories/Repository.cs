namespace DDDLite.Repositories
{
    using DDDLite.Domain;
    using System;
    using System.Threading.Tasks;
    using DDDLite.Specifications;
    using System.Linq;
    using DDDLite.Querying;

    public abstract class Repository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        protected Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IUnitOfWork UnitOfWork { get; protected set; }

        public abstract Task AddAsync(TAggregateRoot entity);
        public abstract Task UpdateAsync(TAggregateRoot entity);
        public abstract Task DeleteAsync(TAggregateRoot entity);
        public abstract Task<TAggregateRoot> GetByIdAsync(TKey id, params string[] includes);

        public virtual IQueryable<TAggregateRoot> Search(params string[] includes)
        {
            return this.Search(Specification<TAggregateRoot>.Any(), SortSpecification<TAggregateRoot>.None, includes);
        }

        public virtual IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, params string[] includes)
        {
            return this.Search(filter, SortSpecification<TAggregateRoot>.None, includes);
        }

        public virtual IQueryable<TAggregateRoot> Search(SortSpecification<TAggregateRoot> sorter, params string[] includes)
        {
            return this.Search(Specification<TAggregateRoot>.Any(), sorter, includes);
        }

        public virtual PagedResult<TAggregateRoot> PagedSearch(int top, int skip, SortSpecification<TAggregateRoot> sorter, params string[] includes)
        {
            return this.PagedSearch(top, skip, Specification<TAggregateRoot>.Any(), sorter, includes);
        }

		public virtual PagedResult<TAggregateRoot> PagedSearch(int top, int skip, Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter, params string[] includes)
		{
            if (sorter == null)
            {
                throw new ArgumentNullException(nameof(sorter));
            }

			var query = this.Search(filter, sorter, includes);
			var count = query.Count();
			var data = query.Skip(skip).Take(top).ToList();
			return new PagedResult<TAggregateRoot>(count, data);
		}

        public abstract IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter, params string[] includes);

        public virtual bool Exists(Specification<TAggregateRoot> filter)
        {
            return this.Search(filter).Any();
        }
    }
}
