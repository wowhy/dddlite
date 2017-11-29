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

  internal class RepositoryQueryContext<TEntity, TKey> : BaseQueryContext<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    private IRepository<TEntity, TKey> repository;

    public RepositoryQueryContext(IRepository<TEntity, TKey> repository, HttpContext context) : base(context)
    {
      this.repository = repository;
    }

    public async override Task<ResponseValue<TEntity>> GetValueAsync(TKey id)
    {
      var Entity = await repository.GetByIdAsync(id, Includes);
      if (Entity == null)
      {
        throw new AggregateRootNotFoundException<TKey>(id);
      }

      var response = new ResponseValue<TEntity>
      {
        Value = Entity
      };

      return response;
    }

    public override ResponseValues<TEntity> GetValues()
    {
      var response = new ResponseValues<TEntity>();
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