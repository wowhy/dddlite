namespace DDDLite.Repository.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Specifications;
    using Domain.Core;
    using Domain.Repositories;
    using global::MongoDB.Driver;
    using Humanizer;

    public class MongoDBRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IMongoDBRepositoryContext dbContext;
        private readonly IMongoCollection<TAggregateRoot> collection;

        public MongoDBRepository(IRepositoryContext context) : base(context)
        {
            this.dbContext = context as IMongoDBRepositoryContext;
            if (this.dbContext == null)
            {
                throw new InvalidCastException("Unable to cast the given repository context instance.");
            }

            this.collection = this.dbContext.GetCollection<TAggregateRoot>();
        }

        public override TAggregateRoot Get(Guid key)
        {
            return this.collection.Find<TAggregateRoot>(k => k.Id == key).FirstOrDefault();
        }

        public override Task<TAggregateRoot> GetAsync(Guid key)
        {
            return this.collection.Find<TAggregateRoot>(k => k.Id == key).FirstOrDefaultAsync();
        }

        public override bool Exists(Specification<TAggregateRoot> specification)
        {
            return this.collection.Count<TAggregateRoot>(specification) > 0;
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
            var query = (IQueryable<TAggregateRoot>)this.collection.AsQueryable();
            if (sortSpecification?.Count > 0)
            {
                var sortSpecifications = sortSpecification.Specifications.ToList();
                var firstSortSpecification = sortSpecifications[0];

                switch (firstSortSpecification.Item2)
                {
                    case Domain.Core.SortDirection.Asc:
                        query = query.OrderBy(firstSortSpecification.Item1);
                        break;

                    case Domain.Core.SortDirection.Desc:
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
                        case Domain.Core.SortDirection.Asc:
                            query = query.OrderBy(spec.Item1);
                            break;

                        case Domain.Core.SortDirection.Desc:
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
            this.dbContext.RegisterInsert(entity);
        }

        public override void Update(TAggregateRoot entity)
        {
            this.dbContext.RegisterUpdate(entity);
        }

        public override void Delete(TAggregateRoot entity)
        {
            this.dbContext.RegisterDelete(entity);
        }
    }
}
