using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDLite.Specifications;
using System.Linq;

namespace DDDLite.Querying
{
    public abstract class QueryService<TReadModel> : IQueryService<TReadModel>
        where TReadModel : class, new()
    {
        public QueryService()
        {
        }

        public virtual Task<int> CountAsync()
        {
            return this.CountAsync(Specification<TReadModel>.Any());
        }

        public abstract Task<int> CountAsync(Specification<TReadModel> specification);

        public abstract Task<TReadModel> GetByIdAsync(Guid key, string[] eagerLoadings);

        public virtual IQueryable<TReadModel> Query(string[] eagerLoadings = null)
        {
            return this.Query(Specification<TReadModel>.Any(), SortSpecification<TReadModel>.None, eagerLoadings);
        }

        public virtual IQueryable<TReadModel> Query(Specification<TReadModel> specification, string[] eagerLoadings = null)
        {
            return this.Query(specification, SortSpecification<TReadModel>.None, eagerLoadings);
        }

        public abstract IQueryable<TReadModel> Query(Specification<TReadModel> specification, SortSpecification<TReadModel> sortSpecification, string[] eagerLoadings = null);
    }
}
