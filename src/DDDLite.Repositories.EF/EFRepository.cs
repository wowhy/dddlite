namespace DDDLite.Repositories.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DDDLite.Core;
    using DDDLite.Repositories;
    using Microsoft.EntityFrameworkCore;
    using DDDLite.Specifications;

    public class EFRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly DbContext dbContext;

        protected DbSet<TAggregateRoot> DbSet => this.dbContext.Set<TAggregateRoot>();

        public EFRepository(IRepositoryContext context) : base(context)
        {
            this.dbContext = (context as EFRepositoryContext)?.Context;

            if (this.dbContext == null)
            {
                throw new RepositoryException("DbContext instance of EntityFrameworkRepositoryContext is invalid.");
            }
        }

        public override TAggregateRoot Get(Guid key)
        {
            return this.DbSet.FirstOrDefault(k => key.Equals(k.Id));
        }

        public override Task<TAggregateRoot> GetAsync(Guid key)
        {
            return this.DbSet.FirstOrDefaultAsync(k => key.Equals(k.Id));
        }

        public override IQueryable<TAggregateRoot> FindAll()
        {
            return this.FindAll(new AnySpecification<TAggregateRoot>());
        }

        public override IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification)
        {
            return this.FindAll(specification, null);
        }

        public override IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification)
        {
            var query = this.DbSet.Where(specification);
            if (sortSpecification?.Count > 0)
            {
                var sortSpecifications = sortSpecification.Specifications.ToList();
                var firstSortSpecification = sortSpecifications[0];

                switch (firstSortSpecification.Item2)
                {
                    case SortDirection.Asc:
                        query = query.OrderBy(firstSortSpecification.Item1);
                        break;

                    case SortDirection.Desc:
                        query = query.OrderByDescending(firstSortSpecification.Item1);
                        break;

                    default:
                        return query;
                }

                for (var i = 1; i < sortSpecifications.Count; i++)
                {
                    var spec = sortSpecifications[i];
                    switch (spec.Item2)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(spec.Item1);
                            break;

                        case SortDirection.Desc:
                            query = query.OrderByDescending(spec.Item1);
                            break;

                        default:
                            continue;
                    }
                }
            }

            return query;
        }

        public override void Insert(TAggregateRoot entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Added;
        }

        public override void Delete(TAggregateRoot entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public override void Update(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            entry.Property(k => k.RowVersion).OriginalValue = entity.RowVersion;
            entry.Property(k => k.RowVersion).CurrentValue = entity.RowVersion + 1;
            entry.State = EntityState.Modified;
        }

        public override bool Exists(Specification<TAggregateRoot> specification)
        {
            return this.DbSet.Where(specification).Any();
        }
    }
}
