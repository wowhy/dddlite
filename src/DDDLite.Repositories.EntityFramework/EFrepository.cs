namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Domain;
    using DDDLite.Querying;
    using DDDLite.Specifications;
    using Microsoft.EntityFrameworkCore;

    public class EFRepository<TAggregateRoot> : Repository<TAggregateRoot>, IEFRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly DbContext context;

        public EFRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            context = unitOfWork as DbContext ?? throw new ArgumentNullException(nameof(context));
        }

        public DbContext Context => this.context;

        public override async Task AddAsync(TAggregateRoot entity)
        {
            await Context.Set<TAggregateRoot>().AddAsync(entity);
        }

        public override Task UpdateAsync(TAggregateRoot entity)
        {
            Context.Update(entity);
            return Task.CompletedTask;
        }

        public override Task DeleteAsync(TAggregateRoot entity)
        {
            Context.Remove(entity);
            return Task.CompletedTask;
        }

        public override Task<TAggregateRoot> GetByIdAsync(Guid id, params string[] includes)
        {
            if (includes != null)
            {
                var query = Context.Set<TAggregateRoot>().AsQueryable();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return query.Where(k => k.Id == id).FirstOrDefaultAsync();
            }

            return Context.Set<TAggregateRoot>().FindAsync(id);
        }

        public override IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter, params string[] includes)
        {
            if (filter == null)
            {
                filter = Specification<TAggregateRoot>.Any();
            }

            if (sorter == null)
            {
                sorter = SortSpecification<TAggregateRoot>.None;
            }

            var query = Context.Set<TAggregateRoot>().Where(filter);
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (sorter.Count > 0)
            {
                var sorts = sorter.Specifications.ToList();
                var orderedQuery = sorts[0].Item2 == SortDirection.Asc
                                           ? query.OrderBy(sorts[0].Item1)
                                           : query.OrderByDescending(sorts[0].Item1);
                for (var i = 1; i < sorts.Count; i++)
                {
                    orderedQuery = sorts[i].Item2 == SortDirection.Asc
                           ? orderedQuery.OrderBy(sorts[i].Item1)
                           : orderedQuery.OrderByDescending(sorts[i].Item1);
                }

                query = orderedQuery;
            }

            return query;
        }
    }
}
