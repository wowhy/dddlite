namespace DDDLite.WebApi.Internal.Query
{
    using System;
    using System.Threading.Tasks;

    using DDDLite.Domain;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Exception;
    using DDDLite.WebApi.Models;
    using DDDLite.WebApi.Internal.Parser;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

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
            if (TryGetParam<bool>(ApiParams.COUNT, out bool hasCount))
            {
                HasCount = hasCount;
            }

            if (TryGetParam<int>(ApiParams.TOP, out int top))
            {
                Top = top;
            }

            if (TryGetParam<int>(ApiParams.SKIP, out int skip))
            {
                Skip = skip;
            }

            if (TryGetParam<string>(ApiParams.INCLUDES, out string includes))
            {
                Includes = includes.Split(',');
            }

            if (TryGetParam<string>(ApiParams.ORDERBY, out string orderBy))
            {
                Sorter = new SorterParser<TAggregateRoot>().Parse(orderBy);
            }

            if (TryGetParam<string>(ApiParams.FILTER, out string filter))
            {
                Filter = new FilterParser<TAggregateRoot>().Parse(filter);
            }

            if (Top != null)
            {
                this.ClientDrivenPaging = true;
            } else if (Skip != null)
            {
                this.ClientDrivenPaging = true;
                if (Top == null)
                {
                    Top = 10;
                }
            }
        }

        private bool TryGetParam<T>(string key, out T value)
        {
            value = default(T);
            var queryString = HttpContext.Request.Query;
            if (queryString.ContainsKey(key))
            {
                if (queryString.TryGetValue(key, out StringValues str))
                {
                    value = (T)Convert.ChangeType(str.ToString(), typeof(T));
                    return true;
                }
                else
                {
                    throw new BadArgumentException(key);
                }
            }

            return false;
        }
    }
}