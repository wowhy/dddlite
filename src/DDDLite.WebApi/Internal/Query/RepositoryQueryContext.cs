namespace DDDLite.WebApi.Internal.Query
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DDDLite.Domain;
    using DDDLite.Exception;
    using DDDLite.Repositories;
    using DDDLite.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    public class RepositoryQueryContext<TAggregateRoot> : BaseQueryContext<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private IRepository<TAggregateRoot> repository;

        public RepositoryQueryContext(IRepository<TAggregateRoot> repository, HttpContext context) : base(context)
        {
            this.repository = repository;
        }

        public async override Task<ResponseValue<TAggregateRoot>> GetValueAsync(Guid id)
        {
            var aggregateRoot = await repository.GetByIdAsync(id);
            if (aggregateRoot == null)
            {
                throw new AggregateNotFoundException(id);
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
            var query = repository.Search(Filter, Sorter);

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
                    query = query.Take(Top.Value);
                }

                response.Value = query.ToList();
            } else if (ServerDrivenPaging) 
            {
                // Not Supported
                throw new NotSupportedException();
            } else 
            {
                response.Value = query.ToList();
            }

            return response;
        }
    }
}