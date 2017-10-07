namespace DDDLite.WebApi.Internal.Query
{
    using System;
    using System.Threading.Tasks;

    using DDDLite.Domain;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Models;

    internal interface IQueryContext<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        bool HasCount { get; }

        bool ClientDrivenPaging { get; }

        bool ServerDrivenPaging { get; }

        int? Top { get; }

        int? Skip { get; }

        string[] Includes { get; }

        SortSpecification<TAggregateRoot> Sorter { get; }

        Specification<TAggregateRoot> Filter { get; }

        Task<ResponseValue<TAggregateRoot>> GetValueAsync(Guid id);

        ResponseValues<TAggregateRoot> GetValues();
    }
}