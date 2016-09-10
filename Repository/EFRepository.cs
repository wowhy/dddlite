namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Domain.Core;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Specifications;

    public class EFRepository<TKey, TEntity> : Repository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly DbContext dbContext;

        protected DbSet<TEntity> DbSet => this.dbContext.Set<TEntity>();

        public EFRepository(IRepositoryContext context) : base(context)
        {
            this.dbContext = (context as EFRepositoryContext)?.Context;

            if (this.dbContext == null)
            {
                throw new RepositoryException("DbContext instance of EntityFrameworkRepositoryContext is invalid.");
            }
        }

        public override TEntity Get(TKey key)
        {
            return this.DbSet.SingleOrDefault();
        }

        public override Task<TEntity> GetAsync(TKey key)
        {
            return this.DbSet.SingleOrDefaultAsync();
        }

        public override IQueryable<TEntity> FindAll()
        {
            return this.FindAll(new AnySpecification<TEntity>());
        }

        public override IQueryable<TEntity> FindAll(Specification<TEntity> specification)
        {
            return this.FindAll(specification, null);
        }

        public override IQueryable<TEntity> FindAll(Specification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification)
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

        public override void Add(TEntity entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Added;
        }

        public override void Remove(TEntity entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public override void Update(TEntity entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public override bool Exists(Specification<TEntity> specification)
        {
            return this.DbSet.Where(specification).Any();
        }
    }
}
