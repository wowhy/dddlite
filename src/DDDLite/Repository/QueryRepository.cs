namespace DDDLite.Repository
{
    using System;
    using System.Linq;


    using Domain;
    using Specifications;

    public abstract class QueryRepository<TAggregateRoot> : IQueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IQueryRepositoryContext context;

        public IQueryRepositoryContext Context => this.context;

        protected QueryRepository(IQueryRepositoryContext context)
        {
            this.context = context;
        }

        public IQueryable<TAggregateRoot> QueryModel => this.Context.GetQueryModel<TAggregateRoot>();

        public virtual TDTO GetById<TDTO>(Guid id) where TDTO : class, new()
        {
            return this.QueryModel.Where(k => k.Id == id).Take(1).ProjectToFirstOrDefault<TDTO>();
        }

        public virtual IQueryable<TDTO> Find<TDTO>() where TDTO : class, new()
        {
            return this.Find<TDTO>(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification) where TDTO : class, new()
        {
            return this.Find<TDTO>(specification, null);
        }

        public virtual IQueryable<TDTO> Find<TDTO>(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification) where TDTO : class, new()
        {
            var query = this.QueryModel.Where(specification);
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
                        return query.ProjectToQueryable<TDTO>();
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

            return query.ProjectToQueryable<TDTO>();
        }
    }
}
