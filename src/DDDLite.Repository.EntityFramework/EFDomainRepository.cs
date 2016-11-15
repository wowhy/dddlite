namespace DDDLite.Repository.EntityFramework
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;


    using DDDLite;
    using DDDLite.Specifications;
    using Events;
    using Messaging;

    public class EFDomainRepository<TAggregateRoot> : DomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly DbContext dbContext;

        public EFDomainRepository(DbContext context, IEventPublisher eventPublisher)
            : base(eventPublisher)
        {
            this.dbContext = context;
            if (this.dbContext == null)
            {
                throw new CoreException("context参数不正确");
            }
        }

        public void Create(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            entry.State = EntityState.Added;
        }

        public void Update(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            var property = entry.Property(k => k.RowVersion);
            property.OriginalValue = entity.RowVersion;
            property.CurrentValue = entity.RowVersion + 1;
            entry.State = EntityState.Modified;
        }

        public void Delete(TAggregateRoot entity)
        {
            var entry = this.dbContext.Entry(entity);
            var property = entry.Property(k => k.RowVersion);
            property.OriginalValue = entity.RowVersion;
            property.CurrentValue = entity.RowVersion + 1;
            entry.State = EntityState.Deleted;
        }

        protected override void DoSave(TAggregateRoot entity)
        {
            var lastEvent = entity.UncommittedEvents.LastOrDefault();
            if (lastEvent != null && lastEvent is DeletedEvent<TAggregateRoot>)
            {
                this.Delete(entity);
            }
            else
            {
                if (EntityState.Detached == this.dbContext.Entry(entity).State)
                {
                    var saved = this.Exist(Specification<TAggregateRoot>.Eval(k => k.Id == entity.Id));
                    if (saved)
                    {
                        this.Update(entity);
                    }
                    else
                    {
                        this.Create(entity);
                    }
                }
                else
                {
                    this.Update(entity);
                }
            }

            this.dbContext.SaveChanges();
        }

        public override bool Exist(Specification<TAggregateRoot> specification)
        {
            return this.dbContext.Set<TAggregateRoot>().Any(specification);
        }

        public override IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification)
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
    }
}