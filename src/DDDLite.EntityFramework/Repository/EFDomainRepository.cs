namespace DDDLite.EntityFramework.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;


    using DDDLite.Domain;
    using DDDLite.Specifications;

    public class EFDomainRepository<TAggregateRoot> : DomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly DbContext dbContext;

        public EFDomainRepository(IDomainRepositoryContext context)
            : base(context)
        {
            this.dbContext = (context as EFDomainRepositoryContext)?.DbContext;
            if (this.dbContext == null)
            {
                throw new CoreException("context参数不正确");
            }
        }

        public override void Create(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            entry.State = EntityState.Added;
        }

        public override void Delete(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            var property = entry.Property(k => k.RowVersion);
            property.OriginalValue = entity.RowVersion;
            property.CurrentValue = entity.RowVersion + 1;
            entry.State = EntityState.Deleted;
        }

        public override bool Exist(Specification<TAggregateRoot> specification)
        {
            return this.dbContext.Set<TAggregateRoot>().Any(specification);
        }

        public override IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification)
        {
            var query = this.dbContext.Set<TAggregateRoot>().Where(specification);
            if (sortSpecification?.Count > 0)
            {
                var sortSpecificationList = sortSpecification.Specifications.ToList();
                var firstSpecification = sortSpecificationList[0];
                switch (firstSpecification.Item2)
                {
                    case SortDirection.Asc:
                        query = query.OrderBy(firstSpecification.Item1);
                        break;

                    case SortDirection.Desc:
                        query = query.OrderByDescending(firstSpecification.Item1);
                        break;

                    default:
                        return query;
                }

                for (var i = 1; i < sortSpecificationList.Count; i++)
                {
                    var spec = sortSpecificationList[0];
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

        public override TAggregateRoot GetById(Guid id)
        {
            return this.dbContext.Set<TAggregateRoot>().FirstOrDefault(k => k.Id == id);
        }

        public override Task<TAggregateRoot> GetByIdAsync(Guid id)
        {
            return this.dbContext.Set<TAggregateRoot>().FirstOrDefaultAsync(k => k.Id == id);
        }

        public override void Update(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            var property = entry.Property(k => k.RowVersion);
            property.OriginalValue = entity.RowVersion;
            property.CurrentValue = entity.RowVersion + 1;
            entry.State = EntityState.Modified;
        }
    }
}