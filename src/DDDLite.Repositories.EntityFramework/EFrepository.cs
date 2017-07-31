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

        public EFRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        public DbContext Context => this.context;

        public override async Task AddAsync(TAggregateRoot entity)
        {
            Context.Set<TAggregateRoot>().Add(entity);
            Context.EnsureLogicalDeleteChanging();
            await Context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(TAggregateRoot entity)
        {
            Context.Remove(entity);
            Context.EnsureLogicalDeleteChanging();
            await Context.SaveChangesAsync();
        }

        public override Task<TAggregateRoot> GetByIdAsync(Guid id)
        {
            return Context.Set<TAggregateRoot>().FindAsync(id);
        }

        public override async Task UpdateAsync(TAggregateRoot entity)
        {
            Context.Update(entity);
            Context.EnsureTrackableChanging();
            await Context.SaveChangesAsync();
        }

        public override IQueryable<TAggregateRoot> Search(Specification<TAggregateRoot> filter, SortSpecification<TAggregateRoot> sorter)
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
