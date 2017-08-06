namespace DDDLite.WebApi.Internal.Query
{
    using System;
    using System.Threading.Tasks;

    using DDDLite.Domain;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    public abstract class BaseQueryContext<TAggregateRoot> : IQueryContext<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private static readonly Specification<TAggregateRoot> DefaultFilter = Specification<TAggregateRoot>.Any();
        private static readonly SortSpecification<TAggregateRoot> DefaultSorter = SortSpecification<TAggregateRoot>.SortByCreatedAtDesc;

        protected BaseQueryContext(HttpContext context)
        {
            this.HttpContext = context;
            
            this.Filter = DefaultFilter;
            this.Sorter = DefaultSorter;

            this.ParseParams();
        }

        public HttpContext HttpContext { get; protected set; }

        public bool HasCount { get; protected set; }
        public bool ClientDrivenPaging { get; protected set; }
        public bool ServerDrivenPaging { get; protected set; }
        public string[] Includes { get; protected set; }
        public SortSpecification<TAggregateRoot> Sorter { get; protected set; }
        public Specification<TAggregateRoot> Filter { get; protected set; }
        public int? Top { get; protected set; }
        public int? Skip { get; protected set; }

        public abstract Task<ResponseValue<TAggregateRoot>> GetValueAsync(Guid id);
        public abstract ResponseValues<TAggregateRoot> GetValues();

        private void ParseParams()
        {
            // TODO: Parse Params
        }
    }
}