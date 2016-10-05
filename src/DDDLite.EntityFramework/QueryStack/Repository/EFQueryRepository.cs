namespace DDDLite.EntityFramework.QueryStack.Repository
{
    using System;
    using System.Linq;

    using DDDLite.Domain;
    using DDDLite.QueryStack.Repository;
    using DDDLite.Specifications;

    public class EFQueryRepository<TAggregateRoot> : QueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public EFQueryRepository(IQueryRepositoryContext context)
            : base(context)
        {
        }

        public override bool Exist(Specification<TAggregateRoot> specification)
        {
            return this.Context.GetQueryableModel<TAggregateRoot>().Any(specification);
        }

        public override IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification)
        {
            var query = this.Context.GetQueryableModel<TAggregateRoot>().Where(specification);
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
            return this.Context.GetQueryableModel<TAggregateRoot>().FirstOrDefault(k => k.Id == id);
        }
    }
}