namespace DDDLite.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Specifications;
    using System.Linq;

    public interface IQueryService<TReadModel>
        where TReadModel : class, new()
    {
        Task<TReadModel> GetByIdAsync(Guid key, string[] eagerLoadings = null);

        Task<int> CountAsync();

        Task<int> CountAsync(Specification<TReadModel> specification);

        IQueryable<TReadModel> Query(string[] eagerLoadings = null);

        IQueryable<TReadModel> Query(Specification<TReadModel> specification, string[] eagerLoadings = null);

        IQueryable<TReadModel> Query(Specification<TReadModel> specification, SortSpecification<TReadModel> sortSpecification, string[] eagerLoadings = null);
    }
}