namespace DDDLite.WebApi.Internal.Query
{
  using System;
  using System.Threading.Tasks;

  using DDDLite.Domain;
  using DDDLite.Specifications;
  using DDDLite.WebApi.Models;

  internal interface IQueryContext<TEntity, TKey>
      where TEntity : class, IEntity<TKey>
      where TKey : IEquatable<TKey>
  {
    bool HasCount { get; }

    bool ClientDrivenPaging { get; }

    bool ServerDrivenPaging { get; }

    int? Top { get; }

    int? Skip { get; }

    string[] Includes { get; }

    SortSpecification<TEntity> Sorter { get; }

    Specification<TEntity> Filter { get; }

    Task<ResponseValue<TEntity>> GetValueAsync(TKey id);

    ResponseValues<TEntity> GetValues();
  }
}