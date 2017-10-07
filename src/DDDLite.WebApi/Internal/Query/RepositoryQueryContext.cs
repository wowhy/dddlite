namespace DDDLite.WebApi.Internal.Query
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DDDLite.Domain;
    using DDDLite.Exception;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Config;
    using DDDLite.WebApi.Exception;
    using DDDLite.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    internal class RepositoryQueryContext<TAggregateRoot> : BaseQueryContext<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private IRepository<TAggregateRoot> repository;

        public RepositoryQueryContext(IRepository<TAggregateRoot> repository, HttpContext context) : base(context)
        {
            this.repository = repository;
        }

        public async override Task<ResponseValue<TAggregateRoot>> GetValueAsync(Guid id)
        {
            var aggregateRoot = await repository.GetByIdAsync(id, Includes);
            if (aggregateRoot == null)
            {
                throw new AggregateRootNotFoundException(id);
            }

            var response = new ResponseValue<TAggregateRoot>
            {
                Value = aggregateRoot
            };

            return response;
        }

        public override ResponseValues<TAggregateRoot> GetValues()
        {
            var response = new ResponseValues<TAggregateRoot>();
            var query = repository.Search(Filter, Sorter, Includes);

            if (HasCount)
            {
                response.Count = query.Count();
            }

            if (ClientDrivenPaging)
            {
                if (Skip != null)
                {
                    query = query.Skip(Skip.Value);
                }
                if (Top != null)
                {
                    if (Top > ApiConfig.Default.MaxQueryCount)
                    {
                        throw new BadArgumentException(ApiParams.TOP);
                    }

                    query = query.Take(Top.Value);
                }

                response.Value = query.ToList();
            }
            else if (ServerDrivenPaging)
            {
                // Not Supported
                throw new NotSupportedException();
            }
            else
            {
                response.Value = query.Take(ApiConfig.Default.MaxQueryCount).ToList();
            }

            return response;
        }
    }
}